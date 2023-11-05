use std::fs::File;
use std::io::{prelude::*, BufReader};
use std::collections::HashSet;
use std::collections::HashMap;

#[derive(PartialEq)]
enum LineType {
    Corrupted,
    Incomplete,
    Complete
}

fn main() {
    let input = read_input();
    let part_one = part_one(&input);
    println!("Part one result {}", part_one); // 268845
    let part_two = part_two(&input);
    println!("Part two result {}", part_two); // 4038824534
}

fn read_input() -> Vec<Vec<char>> {
    let args: Vec<String> = std::env::args().collect();
    let path = std::path::Path::new(&args[1]);
    let file = File::open(&path).unwrap();
    let lines: Vec<Vec<char>> = 
        BufReader::new(file).
        lines().into_iter().
        map(|x| x.unwrap().chars().collect::<Vec<char>>()).collect();
    return lines;
}

fn part_one(input: &Vec<Vec<char>>) -> u64 {
    return
        input.iter().
        map(|x| get_line_score(x)).
        filter(|x| x.0 == LineType::Corrupted).
        fold(0, |acc, next| acc + next.1);        
}

fn part_two(input: &Vec<Vec<char>>) -> u64 {
    let mut lines = 
        input.iter().
        map(|x| get_line_score(x)).
        filter(|x| x.0 == LineType::Incomplete).
        map(|x| x.1).
        collect::<Vec<u64>>();
    
    lines.sort();
    return lines[lines.len() / 2];
}

fn get_line_score(line: &Vec<char>) -> (LineType, u64) { 
    let mut left_set = HashSet::new();
    left_set.insert('(');
    left_set.insert('[');
    left_set.insert('{');
    left_set.insert('<');

    let mut right_match: HashMap<char, char> = HashMap::new();
    right_match.insert(')', '(');
    right_match.insert(']', '[');
    right_match.insert('}', '{');
    right_match.insert('>', '<');

    let mut right_point_match: HashMap<char, u64> = HashMap::new();
    right_point_match.insert(')', 3);
    right_point_match.insert(']', 57);
    right_point_match.insert('}', 1197);
    right_point_match.insert('>', 25137);

    let mut stack: Vec<&char> = Vec::new();
    for next_char in line {
        if left_set.contains(next_char) {
            stack.push(next_char);
        }
        else {
            let from_stack = stack.pop();
            if from_stack.is_some() {
                let from_stack_value = from_stack.unwrap();
                if right_match[next_char] == *from_stack_value {
                    continue;
                }
                else {
                    return (LineType::Corrupted, right_point_match[next_char]);
                }
            }
            else {
                return (LineType::Corrupted, right_point_match[next_char]);
            }
        }
    }

    if stack.is_empty() {
        return (LineType::Complete, 0)
    }

    let mut left_point_match: HashMap<char, u64> = HashMap::new();
    left_point_match.insert('(', 1);
    left_point_match.insert('[', 2);
    left_point_match.insert('{', 3);
    left_point_match.insert('<', 4);

    let mut complete_score = 0;
    while stack.is_empty() == false {
        let next_from_stack = stack.pop().unwrap();
        complete_score = (5 * complete_score) + (left_point_match[next_from_stack]); 
    }
    return (LineType::Incomplete, complete_score);
}
