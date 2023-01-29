use std::env;
use std::fs::File;
use std::io::{prelude::*, BufReader};
use std::path::Path;

fn main() {
    let input = read_input();
    let part_one = part_one(input);
    println!("Part one result {}", part_one);
}

fn read_input() -> Vec<i32> {
    let args: Vec<String> = env::args().collect();
    let path = Path::new(&args[1]);
    let file = File::open(&path).unwrap();
    
    return BufReader::new(file).
        lines().into_iter().
        map(|x| x.unwrap().parse::<i32>().unwrap()).collect();
}

fn part_one(input: Vec<i32>) -> i32 {
    let mut increase_count = 0;
    for i in 1..input.len() {
        let last_val = input[i-1];
        let curr_val = input[i];
        if last_val < curr_val {
            increase_count = increase_count + 1;
        }
    }

    return increase_count;
}