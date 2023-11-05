use std::fs::File;
use std::io::{prelude::*, BufReader};
use std::vec;

fn main() {
    let input = read_input();
    let part_one = part_one(input.clone());
    println!("Part one result {}", part_one); // 3885894
    let part_two = part_two(input.clone());
    println!("Part two result {}", part_two); // 4375225
}

fn read_input() -> Vec<String>{
    let args: Vec<String> = std::env::args().collect();
    let path = std::path::Path::new(&args[1]);
    let file = File::open(&path).unwrap();

    return 
        BufReader::new(file).lines().into_iter().
        map(|x| x.unwrap()).collect();
}

fn part_one(input: Vec<String>) -> i32 {
    let mut bit_count: Vec<i32> = vec![0; input[0].len()];
    for line in input {
        let line_chars: Vec<char> = line.chars().collect();
        for i in 0..line.len() {
            match line_chars[i] {
                '0' => bit_count[i] -=  1,
                '1' => bit_count[i] += 1,
                _  => panic!("Unknown bit")
            }
        }
    }

    let mut gamma_string = String::new();
    let mut epsilon_string = String::new();
    for bit in bit_count {
        if bit > 0 {
            gamma_string.push('1');
            epsilon_string.push('0');
        }
        else if bit < 0 {
            gamma_string.push('0');
            epsilon_string.push('1');
        }
    }
    let gamma_num = isize::from_str_radix(&gamma_string, 2).unwrap();
    let epsilon_num = isize::from_str_radix(&epsilon_string, 2).unwrap();

    return (gamma_num * epsilon_num) as i32;
}

fn part_two(input: Vec<String>) -> i32 {
    let oxygen_func = 
        |zero_list: Vec<String>, one_list: Vec<String>| 
        if zero_list.len() <= one_list.len() {
            return one_list;
        }
        else {
            return zero_list;
        };
    let co2_scrubber_func = 
        |zero_list: Vec<String>, one_list: Vec<String>| 
        if zero_list.len() <= one_list.len() {
            return zero_list;
        }
        else {
            return one_list;
        };
    
    let oxygen_value = process_life_support(&input, oxygen_func);
    let co2_value = process_life_support(&input, co2_scrubber_func);

    let oxygen_num = isize::from_str_radix(&oxygen_value[0], 2).unwrap();
    let c02_num = isize::from_str_radix(&co2_value[0], 2).unwrap();
    return (oxygen_num * c02_num) as i32;
}

fn process_life_support(input: &Vec<String>, life_support_func: fn(Vec<String>,Vec<String>) -> Vec<String>) -> Vec<String> {
    let bits_length = input[0].len();

    let mut rating_list: Vec<String> = input.to_vec();
    for i in 0..bits_length {
        if rating_list.len() == 1 {
            break;
        }
        let mut zero_list: Vec<String> = Vec::new();
        let mut one_list: Vec<String> = Vec::new();
        for bits in rating_list {
            let bits_vec: Vec<char> = bits.chars().collect();
            match bits_vec[i] {
                '0' => zero_list.push(bits.to_string()),
                '1' => one_list.push(bits.to_string()),
                _  => panic!("Unexpected bit")
            }
        }
        rating_list = life_support_func(zero_list, one_list);
    }

    return rating_list;
}