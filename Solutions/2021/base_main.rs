use std::fs::File;
use std::io::{prelude::*, BufReader};

fn main() {
    let input = read_input();
    //let part_one = part_one(&input);
    //println!("Part one result {}", part_one); // 1559
    //let part_two = part_two(&input);
    //println!("Part two result {}", part_two); // 1600
}

fn read_input() -> std::io::Lines<BufReader<File>> {
    let args: Vec<String> = std::env::args().collect();
    let path = std::path::Path::new(&args[1]);
    let file = File::open(&path).unwrap();

    return BufReader::new(file).lines();
}

fn part_one() -> i32 {
    return 0;
}

fn part_two() -> i32 {
    return 0;
}