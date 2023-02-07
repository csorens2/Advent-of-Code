use std::fs::File;
use std::io::{prelude::*, BufReader};
use regex::Regex;

fn main() {
    let input = read_input();
    let part_one = part_one(input.clone());
    println!("Part one result {}", part_one);
    //let part_two = part_two(&input);
    //println!("Part two result {}", part_two);
}

fn read_input() -> (Vec<i32>, Vec<Vec<Vec<(i32, bool)>>>) {
    let args: Vec<String> = std::env::args().collect();
    let path = std::path::Path::new(&args[1]);
    let file = File::open(&path).unwrap();
    let lines = 
        BufReader::new(file).lines().into_iter().
        map(|x| x.unwrap()).
        collect::<Vec<String>>();

    let call_nums = 
        lines[0].split(',').into_iter().
        map(|x| x.parse::<i32>().unwrap()).collect::<Vec<i32>>();

    let grid_side = 5;
    let mut grids: Vec<Vec<Vec<(i32,bool)>>> = Vec::new();
    for i in (2..lines.len()).step_by(grid_side + 1) {
        let mut next_grid: Vec<Vec<(i32,bool)>> = Vec::new();
        for j in i..i+grid_side {
            let re = Regex::new(r"(\d+)").unwrap();
            let mut grid_row: Vec<(i32,bool)> = Vec::new();
            for capture in re.captures_iter(&lines[j]) {
                grid_row.push((capture[1].parse::<i32>().unwrap(), false));
            }
            next_grid.push(grid_row)
        }
        grids.push(next_grid)
    }
    return (call_nums, grids);
}

fn part_one(input: (Vec<i32>, Vec<Vec<Vec<(i32, bool)>>>)) -> i32 {
    let (nums, grids) = input;

    for num in nums {
        for i in 0..grids.len() {
            grids[i] = update_grid(num, &mut grids[i]);

            if check_grid(&grids[i]) {
                return sum_grid(&grids[i]) * num;
            }
        }
    }

    panic!("Should have sumgrid");
}

fn update_grid(num: i32, grid: & mut Vec<Vec<(i32,bool)>>) -> &mut Vec<Vec<(i32,bool)>> {

    for x in 0..grid.len() {
        for y in 0..grid.len() {
            if grid[x][y].0 == num {
                grid[x][y].1 = true;
            }
        }
    }
    return grid;
}

fn check_grid(grid: &Vec<Vec<(i32,bool)>>) -> bool {
    let grid_len = grid[0].len();

    // Check row
    for y in 0..grid_len {
        let mut row_called = true;
        for x in 0..grid_len - 1 {
            if grid[x][y].1 == false {
                row_called = false;
                break;
            }
        }
        if row_called {
            return true;
        }
    }

    // Check columns
    for x in 0..grid_len- 1 {
        let mut col_called = true;
        for y in 0..grid_len - 1 {
            if grid[x][y].1 == false {
                col_called = false;
                break;
            }
        }
        if col_called {
            return true;
        }
    }

    // Check diags
    let diags = vec![
        vec![(0,0),(1,1),(2,2),(3,3),(4,4)], 
        vec![(4,0),(3,1),(2,2),(1,3),(0,4)]
    ];

    for diag in diags {
        let mut diag_called = true;
        for (diag_x, diag_y) in diag {
            if grid[diag_x][diag_y].1 == false {
                diag_called = false;
                break;
            }
        }
        if diag_called {
            return true;
        }
    }
    return false;
}

fn sum_grid(grid: &Vec<Vec<(i32,bool)>>) -> i32 {

    let mut to_return = 0;
    for row in grid {
        for (val, used) in row {
            if *used {
                to_return += val;
            }
        }
    }

    return to_return;
}

fn part_two() -> i32 {
    return 0;
}