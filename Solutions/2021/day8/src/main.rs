use std::fs::File;
use std::io::{prelude::*, BufReader};
use regex::Regex;
use regex::Captures;
use regex::bytes::Match;

fn main() {
    let input = read_input();
    let part_one = part_one(&input); // 521
    println!("Part one result {}", part_one); 
    let part_two = part_two(&input);
    println!("Part two result {}", part_two);
}

fn read_input() -> Vec<(Vec<String>,Vec<String>)> {
    let args: Vec<String> = std::env::args().collect();
    let path = std::path::Path::new(&args[1]);
    let file = File::open(&path).unwrap();

    let lines: Vec<String> = 
        BufReader::new(file).
        lines().into_iter().
        map(|x| x.unwrap()).collect();

    let mut signals: Vec<(Vec<String>,Vec<String>)> = Vec::new();
    for line in lines {
        let re = Regex::new(r"(\w+)").unwrap();
        let mut input_strings: Vec<String> = Vec::new();
        let mut output_strings: Vec<String> = Vec::new();
        let input_iter = re.find_iter(&line).take(10);
        let output_iter = re.find_iter(&line).skip(10).take(4);
        for input_str in input_iter {
            input_strings.push(input_str.as_str().to_string());
        }
        for output_str in output_iter {
            output_strings.push(output_str.as_str().to_string());
        }
        
        signals.push((input_strings, output_strings));
    }

    return signals;
}

fn part_one(input: &Vec<(Vec<String>,Vec<String>)>) -> i32 {
    let mut unique_output_count = 0;
    let unique_lengths = vec![2,4,3,7];
    for (_, output_vec) in input {
        unique_output_count += 
            output_vec.into_iter().
            filter(|x| unique_lengths.contains(&((x).len() as i32))).
            collect::<Vec<&String>>().
            len();
    }

    return unique_output_count as i32;
}

fn part_two(input: &Vec<(Vec<String>,Vec<String>)>) -> i32 {
    let pos_1: char;
    let pos_2: char;
    let mut pos_3: char;
    let pos_4: char;
    let pos_5: char;
    let pos_6: char;
    let mut pos_7: char;
    for (input_vec, output_vec) in input {
        let input_chars = 
            input_vec.into_iter().
            map(|x| x.clone().chars().collect::<Vec<char>>())
            .collect::<Vec<Vec<char>>>();
        
        let one_str = 
            input_chars.into_iter().
            find(|x| 2 == (*x).len()).unwrap();
        let one_str_1 = one_str[0];
        let one_str_2 = one_str[1];
    }
    return 0;
}