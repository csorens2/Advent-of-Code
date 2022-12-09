use std::{env};
use std::path::Path;
use std::fs::File;
use std::io::{BufRead, BufReader};
use std::cmp::max;

fn main() {
    let args: Vec<String> = env::args().collect();
    let tree_grid: Vec<Vec<u32>> = ParseInput(&args[1]);

    // Expected: 1843
    let part_1_result: u32 = Part1(&tree_grid);
    println!("Part 1 Result: '{}'", part_1_result);
    // Expected: 180000
    let part_2_result: u32 = Part2(&tree_grid);
    println!("Part 2 Result: '{}'", part_2_result);
}

fn ParseInput(input_path: &String) -> Vec<Vec<u32>> {
    let path: &Path = Path::new(input_path);
    let file: File = File::open(&path).unwrap();
    let lines = BufReader::new(file).lines();

    let mut base_vec: Vec<Vec<u32>> = Vec::new();
    for line in lines {
        let result_line: String = line.unwrap();
        let line_trees_vec: Vec<u32> = 
            result_line.chars().
            map(|x: char| x.to_digit(10).unwrap()).
            collect();
        base_vec.push(line_trees_vec);
    }
    return base_vec;
}

fn Part1(tree_grid: &Vec<Vec<u32>>) -> u32 {
    let mut tree_count: u32 = 0;
    for y in 0..(&tree_grid).len() {
        for x in 0..(&tree_grid[y]).len() {
            if TreeVisibleFromEdge(((y as i32),(x as i32)), tree_grid) {
                tree_count+=1;
            }
        }
    }
    return tree_count; 
}

fn Part2(tree_grid: &Vec<Vec<u32>>) -> u32 {
    let mut best_scenic: u32 = 0;
    for y in 0..(&tree_grid).len() {
        for x in 0..(&tree_grid[y]).len() {
            let current_scenic = GetScenicScore(((y as i32),(x as i32)), tree_grid);
            best_scenic = max(best_scenic, current_scenic);
        }
    }
    return best_scenic;
}

fn GetMoveFuncs() -> Vec<fn((i32,i32)) -> (i32,i32)> {
    return vec![MoveUp, MoveRight, MoveDown, MoveLeft];
}

fn MoveUp(point:(i32,i32)) -> (i32,i32) { 
    return (point.0-1, point.1); 
}
fn MoveRight(point:(i32,i32)) -> (i32,i32) { 
    return (point.0, point.1+1); 
}
fn MoveDown(point:(i32,i32)) -> (i32,i32) { 
    return (point.0+1, point.1); 
}
fn MoveLeft(point:(i32,i32)) -> (i32,i32) { 
    return (point.0, point.1-1); 
}

fn CanTraverseToEdge(pos: (i32,i32), init_height: u32, tree_grid: &Vec<Vec<u32>>, traversal_func: fn((i32,i32)) -> (i32,i32)) -> bool {
    let y_pos: i32 = pos.0;
    let x_pos: i32 = pos.1;
    if y_pos < 0 || (y_pos as usize) >= tree_grid.len() ||
        x_pos < 0 || (x_pos as usize) >= tree_grid[(y_pos as usize)].len() {
        
        return true;
    }

    let curr_height: u32 = tree_grid[(y_pos as usize)][(x_pos as usize)];
    if curr_height >= init_height {
        return false;
    }
    let next_pos: (i32, i32) = traversal_func((y_pos, x_pos));
    return CanTraverseToEdge(next_pos, init_height, tree_grid, traversal_func)
}

fn TreeVisibleFromEdge(pos: (i32,i32), tree_grid: &Vec<Vec<u32>>) -> bool {
    for move_func in GetMoveFuncs() {
        let next_pos: (i32,i32) = move_func(pos);
        let init_height: u32 = tree_grid[pos.0 as usize][pos.1 as usize];
        let result: bool = CanTraverseToEdge(next_pos, init_height, tree_grid, move_func);
        if result == true {
            return true;
        }
    }
    return false;
}

fn CountTreesInDirection(pos: (i32,i32), init_height: u32, tree_grid: &Vec<Vec<u32>>, traversal_func: fn((i32,i32)) -> (i32,i32)) -> u32 {
    let y_pos: i32 = pos.0;
    let x_pos: i32 = pos.1;
    if y_pos < 0 || (y_pos as usize) >= tree_grid.len() ||
        x_pos < 0 || (x_pos as usize) >= tree_grid[(y_pos as usize)].len() {

        return 0;
    }

    let curr_height: u32 = tree_grid[(y_pos as usize)][(x_pos as usize)];
    if curr_height >= init_height {
        return 1;
    }
    let next_pos: (i32, i32) = traversal_func((y_pos, x_pos));
    return 1 + CountTreesInDirection(next_pos, init_height, tree_grid, traversal_func)
}

fn GetScenicScore(pos: (i32,i32), tree_grid: &Vec<Vec<u32>>) -> u32 {
    let mut scenic_view_list: Vec<u32> = Vec::new();
    for move_func in GetMoveFuncs() {
        let init_pos: (i32,i32) = move_func(pos);
        let init_height: u32 = tree_grid[pos.0 as usize][pos.1 as usize];
        let result: u32 = CountTreesInDirection(init_pos, init_height, tree_grid, move_func);
        scenic_view_list.push(result);
    }
    let score: u32 = scenic_view_list.into_iter().fold(1, |acc, next| acc * next);
    return score
}