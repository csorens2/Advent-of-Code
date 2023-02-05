use std::fs::File;
use std::io::{prelude::*, BufReader};
use regex::Regex;

#[derive(PartialEq)]
enum Direction {
    Forward,
    Down,
    Up
}

struct Command {
    direction: Direction,
    distance: i32
}

fn main() {
    let input = read_input();
    let part_one = part_one(&input);
    println!("Part one result {}", part_one); // 1938402
    let part_two = part_two(&input);
    println!("Part two result {}", part_two); // 1947878632
}

fn read_input() -> Vec<Command> {
    let args: Vec<String> = std::env::args().collect();
    let path = std::path::Path::new(&args[1]);
    let file = File::open(&path).unwrap();
    let lines: Vec<String> = 
        BufReader::new(file).lines().into_iter().
        map(|x| x.unwrap()).collect();

    let re = Regex::new(r"(\w+) (\d+)").unwrap();
    let mut to_return: Vec<Command> = Vec::new();
    for line in lines {
        let capture = re.captures(&line).unwrap();
        let direction: Direction = match &capture[1] {
            "forward" => Ok(Direction::Forward),
            "down" => Ok(Direction::Down),
            "up" => Ok(Direction::Up),
            _ => Err("Invalid Direction")
        }.unwrap();
        to_return.push(Command { direction: direction, distance: capture[2].parse::<i32>().unwrap() })
    }

    return to_return;
}

fn part_one(input: &Vec<Command>) -> i32 {
    let mut x_pos = 0;
    let mut y_pos = 0;
    for command in input {
        let distance = command.distance;
        match command.direction {
            Direction::Forward=> x_pos += distance,
            Direction::Up=>y_pos -= distance,
            _ => y_pos += distance,
        }
    }

    return x_pos * y_pos;
}

fn part_two(input: &Vec<Command>) -> i32 {
    let mut aim = 0;
    let mut y_pos = 0;
    let mut x_pos = 0;
    for command in input {
        let distance = command.distance;
        if command.direction == Direction::Forward {
            x_pos += distance;
            y_pos += aim * distance
        }
        if command.direction == Direction::Up {
            aim -= distance;
        }
        if command.direction == Direction::Down {
            aim += distance;
        }
    }

    return x_pos * y_pos;
}