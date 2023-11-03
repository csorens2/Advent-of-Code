use std::fs::File;
use std::io::{prelude::*, BufReader};

fn main() {
    let input = read_input();
    let part_one = part_one(&input);
    println!("Part one result {}", part_one); // 349812
    let part_two = part_two(&input);
    println!("Part two result {}", part_two); // 99763899
}

fn read_input() -> Vec<i32> {
    let args: Vec<String> = std::env::args().collect();
    let path = std::path::Path::new(&args[1]);
    let file = File::open(&path).unwrap();
    let lines: Vec<String> = 
        BufReader::new(file).
        lines().into_iter().
        map(|x| x.unwrap()).collect();

    let input: Vec<i32> = 
        lines[0].split(',').into_iter().
        map(|x| x.parse::<i32>().unwrap()).collect();

    return input;
}

fn part_one(input: &Vec<i32>) -> i32 {
    let mut crabs = input.clone();
    crabs.sort();

    let mut left_crabs_count = 0;
    let mut right_crabs_count = crabs.len() as i32;
    let mut left_crabs_fuel = 0;
    let mut right_crabs_fuel = 
        crabs.iter().
        fold(0, |acc, next| acc + (next - crabs[0]));
    let mut next_crab = 0;
    let mut least_fuel = left_crabs_fuel + right_crabs_fuel;

    for pos in crabs[0]..(crabs[crabs.len() - 1] + 1) {
        while next_crab < crabs.len() && pos == crabs[next_crab] {
            left_crabs_count += 1;
            right_crabs_count -= 1;
            next_crab += 1;
        }
        left_crabs_fuel += left_crabs_count;
        right_crabs_fuel -= right_crabs_count;
        least_fuel = std::cmp::min(least_fuel, left_crabs_fuel + right_crabs_fuel);
    }
    return least_fuel;
}

fn part_two(input: &Vec<i32>) -> i32 {
    let mut crabs = input.clone();
    crabs.sort();

    let min_pos = crabs[0];
    let max_pos = crabs[crabs.len() - 1];

    let mut crab_stacks: Vec<(i32,i32)> = Vec::new();
    let mut curr_crab_pos = crabs[0];
    let mut curr_crab_count = 1;
    for crab in crabs.iter().skip(1) {
        if *crab != curr_crab_pos {
            crab_stacks.push((curr_crab_pos, curr_crab_count));
            curr_crab_pos = *crab;
            curr_crab_count = 1;
        }
        else {
            curr_crab_count += 1;
        }
    }
    crab_stacks.push((curr_crab_pos, curr_crab_count));

    let mut fuel_vec = vec![0; (max_pos+1) as usize];
    for (pos, crab_num) in crab_stacks.iter() {
        let ranges = vec![
            (*pos+1..max_pos+1).collect::<Vec<i32>>(),
            (min_pos..*pos).rev().collect::<Vec<i32>>()];
        for range in ranges {
            let mut curr_fuel = 1;
            let mut cum_fuel = 0;
            for i in range {
                cum_fuel += curr_fuel;
                curr_fuel += 1;
                fuel_vec[i as usize] += crab_num * cum_fuel;
            }
        }
    }
    fuel_vec.sort();
    return fuel_vec[0];
}