use std::fs::File;
use std::io::{prelude::*, BufReader};
// use regex::Regex;
// cargo add regex

fn main() {
    let input = read_input();
    let part_one = part_one(&input);
    println!("Part one result {}", part_one); // 1741
    let part_two = part_two(&input);
    println!("Part two result {}", part_two); // 440
}

fn read_input() -> Vec<Vec<u32>> {
    let args: Vec<String> = std::env::args().collect();
    let path = std::path::Path::new(&args[1]);
    let file = File::open(&path).unwrap();
    
    let ints = 
        BufReader::new(file).
        lines().into_iter().
        map(|x| 
            x.unwrap().chars().
            map(|y| y.to_digit(10).unwrap()).collect::<Vec<u32>>()).
        collect::<Vec<Vec<u32>>>();
    return ints;
}

fn part_one(input: &Vec<Vec<u32>>) -> i32 {

    let total_runs = 100;
    let mut grid = input.clone();
    let mut flash_count = 0;

    for _ in 0..total_runs {
        flash_count += process_grid(&mut grid);
    }
    
    return flash_count;
}

fn process_grid(grid: &mut Vec<Vec<u32>>) -> i32 {
    let mut flash_count = 0;

    // Increment the bulbs
    let mut flash_grid: Vec<(usize,usize)> = Vec::new();
    for i in 0..grid.len() {
        for j in 0..grid[0].len() {
            grid[i][j] += 1;
            if grid[i][j] == 10 {
                flash_grid.push((i,j));
            }
        }
    }

    // Process a flash, or the fallout of a flash
    while !flash_grid.is_empty() {
        let (flash_y, flash_x) = flash_grid.pop().unwrap();
        // This space has already processed a flash. Nothing else will happen to it this iteration
        if grid[flash_y][flash_x] == 0 {
            continue;
        }
        else
        {
            grid[flash_y][flash_x] += 1;
        } 

        // Process a flash
        if grid[flash_y][flash_x] >= 10 {
            grid[flash_y][flash_x] = 0;

            flash_count += 1;

            // Starts at 12 o'clock, clockwise
            if flash_y != 0 {
                flash_grid.push((flash_y - 1, flash_x));
            }
            if flash_y != 0 && flash_x < grid[flash_y].len() - 1{
                flash_grid.push((flash_y - 1, flash_x + 1));
            }
            if flash_x < grid[flash_y].len() - 1{
                flash_grid.push((flash_y, flash_x + 1));
            }
            if flash_y < grid.len() - 1 && flash_x < grid[flash_y].len() - 1{
                flash_grid.push((flash_y + 1, flash_x + 1));
            }
            if flash_y < grid.len() - 1 {
                flash_grid.push((flash_y + 1, flash_x));
            }
            if flash_y < grid.len() - 1 && flash_x != 0 {
                flash_grid.push((flash_y + 1, flash_x - 1));
            }
            if flash_x != 0 {
                flash_grid.push((flash_y, flash_x - 1));
            }
            if flash_y != 0 && flash_x != 0 {
                flash_grid.push((flash_y - 1, flash_x - 1));
            }
        }            
    }

    return flash_count;
}

fn part_two(input: &Vec<Vec<u32>>) -> i32 {

    let mut grid = input.clone();
    let mut run_count = 0;
    while process_grid(&mut grid) != 100 {
        run_count += 1;
    }

    return run_count + 1;
}