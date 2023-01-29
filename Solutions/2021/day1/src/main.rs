use std::env;
use std::fs::File;
use std::io::{prelude::*, BufReader};
use std::path::Path;

fn main() {
    let input = read_input();
    let part_one = part_one(&input);
    println!("Part one result {}", part_one);
    let part_two = part_two(&input);
    println!("Part two result {}", part_two);
}

fn read_input() -> Vec<i32> {
    let args: Vec<String> = env::args().collect();
    let path = Path::new(&args[1]);
    let file = File::open(&path).unwrap();

    return BufReader::new(file).
        lines().into_iter().
        map(|x| x.unwrap().parse::<i32>().unwrap()).collect();
}

fn part_one(input: &Vec<i32>) -> i32 {
    return count_depth_increases(input);
}

fn part_two(input: &Vec<i32>) -> i32 {
    let depth_measures = combine_depths(input, 3);
    return count_depth_increases(&depth_measures);
}

fn count_depth_increases(input: &Vec<i32>) -> i32 {
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

fn combine_depths(input: &Vec<i32>, depth_length:usize) -> Vec<i32> {
    let mut to_return: Vec<i32> = Vec::new();
    for i in 0..(input.len() - depth_length + 1) {
        to_return.push(0);
        for j in i..(i + depth_length) {
            to_return[i] += input[j];
        }
    }

    return to_return;
}