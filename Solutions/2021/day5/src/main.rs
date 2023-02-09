use std::collections::btree_map::Range;
use std::fs::File;
use std::io::{prelude::*, BufReader};
use regex::Regex;
use std::collections::HashMap;
use std::cmp;
use std::ops;

fn main() {
    let input = read_input();
    let part_one = part_one(input.clone());
    println!("Part one result {}", part_one); // 7269
    //let part_two = part_two(&input);
    //println!("Part two result {}", part_two); 
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
    let mut grid: HashMap<(i32,i32), bool> = HashMap::new();
    for ((x1,y1),(x2,y2)) in input {
        let mut x_range: Vec<i32> = Vec::new();
        let mut y_range: Vec<i32> = Vec::new();
        if x1 != x2 && y1 != y2 {
            continue;
        }
        if x1 == x2 {
            x_range = vec![x1];
        } 
        else {
            let x_min = cmp::min(x1,x2);
            let x_max = cmp::max(x1,x2);
            x_range = (x_min..x_max + 1).collect();
        }
        if y1 == y2 {
            y_range = vec![y1];
        }
        else {
            let y_min = cmp::min(y1,y2);
            let y_max = cmp::max(y1,y2);
            y_range = (y_min..y_max + 1).collect();
        }

        fill_grid(&x_range, &y_range, &mut grid);
    }

    let overlap_count = 
        grid.values().
        filter(|x| **x == true).
        collect::<Vec<&bool>>().
        len();

    return overlap_count as i32;
}

fn fill_grid(x_range: &Vec<i32>, y_range: &Vec<i32>, grid: &mut HashMap<(i32,i32), bool>) {
    for x in x_range {
        for y in y_range {
            if grid.contains_key(&(*x,*y)) {
                grid.insert((*x, *y), true);
            }
            else {
                grid.insert((*x,*y), false);
            }
        }
    }
}

fn part_two() -> i32 {
    return 0;
}