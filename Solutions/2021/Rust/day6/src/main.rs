use std::fs::File;
use std::io::{prelude::*, BufReader};

fn main() {
    let input = read_input();
    let part_one = part_one(&input);
    println!("Part one result {}", part_one);  // 350605
    let part_two = part_two(&input);
    println!("Part two result {}", part_two);
}

fn read_input() -> Vec<i32> {
    let args: Vec<String> = std::env::args().collect();
    let path = std::path::Path::new(&args[1]);
    let file = File::open(&path).unwrap();
    let lines: Vec<String> = 
        BufReader::new(file).
        lines().into_iter().
        map(|x| x.unwrap()).collect();
    let nums: Vec<i32> = 
        lines[0].split(',').
        map(|x| x.parse::<i32>().unwrap()).collect();

    return nums;
}

fn part_one(input: &Vec<i32>) -> u64 {
    return count_fish(input, 80);
}

fn part_two(input: &Vec<i32>) -> u64 {
    return count_fish(input, 256);
}

fn count_fish(input: &Vec<i32>, days: i32) -> u64 {
    let mut fish_vec: Vec<u64> = vec![0,0,0,0,0,0,0,0,0];
    for num in input {
        fish_vec[*num as usize] += 1;
    }

    for _ in 0..days {
        let zero_fish = fish_vec[0];
        for i in 0..fish_vec.len() - 1 {
            fish_vec[i] = fish_vec[i + 1];
        }

        fish_vec[8] = zero_fish;
        fish_vec[6] += zero_fish;
    }

    let fish_sum = 
        fish_vec.into_iter().
        fold(0, |acc, next| acc + next);

    return fish_sum;
}