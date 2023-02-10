use std::fs::File;
use std::io::{prelude::*, BufReader};

fn main() {
    let input = read_input();
    //let part_one = part_one(&input);
    //println!("Part one result {}", part_one); 
    //let part_two = part_two(&input);
    //println!("Part two result {}", part_two);
}

fn read_input() -> Vec<i32> {
    let args: Vec<String> = std::env::args().collect();
    let path = std::path::Path::new(&args[1]);
    let file = File::open(&path).unwrap();
    let lines: Vec<i32> = 
        BufReader::new(file).
        lines().into_iter().
        map(|x| x.unwrap().parse::<i32>().unwrap()).collect();

    return lines;
}

fn part_one() -> i32 {
    return 0;
}

fn part_two() -> i32 {
    return 0;
}