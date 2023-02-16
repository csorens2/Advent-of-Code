use std::fs::File;
use std::io::{prelude::*, BufReader};
// use regex::Regex;
// cargo add regex

fn main() {
    let input = read_input();
    let part_one = part_one(&input);
    println!("Part one result {}", part_one); // 550
    let part_two = part_two(&input);
    println!("Part two result {}", part_two);
}

fn read_input() -> Vec<Vec<u32>> {
    let args: Vec<String> = std::env::args().collect();
    let path = std::path::Path::new(&args[1]);
    let file = File::open(&path).unwrap();
    let lines: Vec<Vec<u32>> = 
        BufReader::new(file).
        lines().into_iter().
        map(|x| 
            x.unwrap().chars().
            map(|y| y.to_digit(10).unwrap()).collect::<Vec<u32>>()).collect();

    return lines;
}

fn is_low_point(grid: &Vec<Vec<u32>>, y: usize, x:usize) -> bool {
    let mut points: Vec<(usize,usize)> = Vec::new();
    if y != 0 {
        points.push((y - 1, x));
    }
    if y != grid.len() - 1 {
        points.push((y + 1, x));
    }
    if x != 0 {
        points.push((y, x - 1));
    }
    if x != grid[0].len() - 1 {
        points.push((y, x + 1));
    }

    let base_height = grid[y][x];
    
    for (y_point, x_point) in points {
        if grid[y_point][x_point] <= base_height {
            return false; 
        }
    }
    return true;
}

fn part_one(input: &Vec<Vec<u32>>) -> u32 {
    let mut total_risk = 0;

    for (y, _) in input.iter().enumerate() {
        for (x, _) in input[y].iter().enumerate() {
            if is_low_point(input, y, x) {
                total_risk += 1 + input[y][x];
            }
        }
    }

    return total_risk;
}

fn get_basin_size(grid: &Vec<Vec<u32>>, used_spaces: &mut Vec<Vec<bool>>, y_curr: usize, x_curr: usize, expected_height: u32) -> u32 {

    if grid[y_curr][x_curr] == 9 {
        return 0;
    }
    if grid[y_curr][x_curr] != expected_height {
        return 0;
    }
    if used_spaces[y_curr][x_curr] {
        return 0;
    }

    let mut next_points: Vec<(usize,usize)> = Vec::new();
    if y_curr != 0 {
        next_points.push((y_curr - 1, x_curr));
    }
    if y_curr != grid.len() - 1 {
        next_points.push((y_curr + 1, x_curr));
    }
    if x_curr != 0 {
        next_points.push((y_curr, x_curr - 1));
    }
    if x_curr != grid[0].len() - 1 {
        next_points.push((y_curr, x_curr + 1));
    }

    used_spaces[y_curr][x_curr] = true;

    return 
        1 + 
        next_points.iter().
        fold(0, |acc, (next_y,next_x)| acc + get_basin_size(grid, used_spaces, *next_y, *next_x, 1 + expected_height));

}

fn part_two(input: &Vec<Vec<u32>>) -> u32 {
    let mut basin_sizes: Vec<u32> = Vec::new();
    for (y, _) in input.iter().enumerate() {
        for (x, _) in input[y].iter().enumerate() {
            if is_low_point(input, y, x) {
                let mut used_spaces = vec![vec![false;input[0].len()];input.len()];
                let basin_size = get_basin_size(input,&mut used_spaces, y, x, input[y][x]);
                basin_sizes.push(basin_size);
            }
        }
    }

    basin_sizes.sort_by(|a,b| b.cmp(a));
    return 
        basin_sizes.iter().
        take(3).
        fold(1, |acc, next| acc * next);
}