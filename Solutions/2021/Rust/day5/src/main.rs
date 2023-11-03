use std::fs::File;
use std::io::{prelude::*, BufReader};
use regex::Regex;
use std::collections::HashMap;

fn main() {
    let input = read_input();
    let part_one = part_one(input.clone());
    println!("Part one result {}", part_one); // 7269
    let part_two = part_two(input.clone());
    println!("Part two result {}", part_two); // 21140
}

fn read_input() -> Vec<((i32,i32),(i32,i32))> {
    let args: Vec<String> = std::env::args().collect();
    let path = std::path::Path::new(&args[1]);
    let file = File::open(&path).unwrap();
    let lines = 
        BufReader::new(file).lines().
        map(|x| x.unwrap()).
        collect::<Vec<String>>();
    
    let mut vent_lines: Vec<((i32,i32),(i32,i32))> = Vec::new();
    for line in lines {
        let re = Regex::new(r"(\d+),(\d+) -> (\d+),(\d+)").unwrap();
        let capture = re.captures(&line).unwrap();
        vent_lines.push((
            (capture[1].parse::<i32>().unwrap(),capture[2].parse::<i32>().unwrap()),
            (capture[3].parse::<i32>().unwrap(),capture[4].parse::<i32>().unwrap())));
    }

    return vent_lines;
}

fn part_one(input: Vec<((i32,i32),(i32,i32))>) -> i32 {
    return process_points(input, false);
}

fn part_two(input: Vec<((i32,i32),(i32,i32))>) -> i32 {
    return process_points(input, true);
}

fn process_points(input: Vec<((i32,i32),(i32,i32))>, process_diag: bool) -> i32 {
    let mut grid: HashMap<(i32,i32), bool> = HashMap::new();
    for ((x1,y1),(x2,y2)) in input {
        if x1 != x2 && y1 != y2 && !process_diag {
            continue;
        }

        let mut curr_x: i32;
        let mut curr_y: i32;
        let end_x: i32;
        let end_y: i32;
        if x1 <= x2 {
            (curr_x, curr_y) = (x1,y1);
            (end_x, end_y) = (x2,y2);
        }
        else {
            (end_x, end_y) = (x1,y1);
            (curr_x, curr_y) = (x2,y2);
        }

        let mut to_insert:Vec<(i32,i32)> = vec![(end_x, end_y)];
        while (curr_x, curr_y) != (end_x, end_y) {
            to_insert.push((curr_x, curr_y));
            if curr_x != end_x {
                curr_x += 1;
            }

            if curr_y < end_y {
                curr_y += 1;
            }
            else if curr_y > end_y{
                curr_y -= 1;
            }
        }

        for (insert_x, insert_y) in to_insert {
            if grid.contains_key(&(insert_x,insert_y)) {
                grid.insert((insert_x,insert_y), true);
            }
            else {
                grid.insert((insert_x,insert_y), false);
            }
        }
    }

    let overlap_count = 
        grid.values().
        filter(|x| **x == true).
        collect::<Vec<&bool>>().
        len();

    return overlap_count as i32;
}