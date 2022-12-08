use std::env;
use std::fs::{File, read};
use std::io::{BufRead, BufReader};
use std::path::Path;
use std::ptr;
use std::option::Option;
use std::collections::HashMap;
use std::str::SplitWhitespace;

fn main() {
    println!("Turns out that working with trees sucks in rust. Who knew?");

    ParseInput();

}

fn ParseInput() -> Directory {

    let args: Vec<String> = env::args().collect();
    let path: &Path = Path::new(&args[1]);
    let file: File = File::open(&path).unwrap();

    let mut last_directory: Option<&Directory> = None;
    let starter_directory: Directory = Directory { name: "/".to_string(), files: Vec::new(), directories: Vec::new() };
    let mut current_directory: &Directory = &starter_directory;

    let change_directory_operation: &str = "$ cd ";
    let list_directory_operation: &str = "$ ls";
    let new_operation_char: &str = "$";
    let directory_start: &str = "dir ";

    let lineIter = BufReader::new(file).lines().enumerate().step_by(1);

    for(index, line) in lineIter {
        let line: String = line.unwrap();

        // Current processing an 'ls' command
        if !line.starts_with(new_operation_char) {
            let lineSplit: Vec<&str>  = line.split(" ").collect();
            let firstPart = lineSplit[0];
            let secondPart = lineSplit[1];
            
            // File
            if(firstPart.parse::<u32>().is_ok()) {
                let fileSize: u32 = firstPart.parse::<u32>().unwrap();
                let newFile: DirectoryFile = DirectoryFile { name: secondPart.to_string(), size: fileSize };
                &current_directory.files.push(newFile);
            }
            else { // New dir
                let newDir: Directory = Directory { name: secondPart.to_string(), files: Vec::new(), directories: Vec::new() };
                &current_directory.directories.push(newDir);
            }
        }

        // Process a new Command
        if line.starts_with(new_operation_char) {
            // No actual work needs to be done for list commands
            if line.starts_with(list_directory_operation) {
                continue;
            }
            else if line.starts_with(change_directory_operation) {
                let targetDir: &str = &line[directory_start.len()+1..];

                last_directory = Some(current_directory);
                // let stuff = current_directory.directories.into_iter().find(|&x| x.name == targetDir.to_string()).unwrap();
            }
        }

        println!("{}. {}", index + 1, line);
    }


    let testString = "".to_string();
    let testDirectory = Directory {name: testString, files: Vec::new(), directories: Vec::new()};
    return testDirectory;
}

struct Directory {
    name: String,
    files:Vec<DirectoryFile>,
    directories:Vec<Directory>
}

struct DirectoryFile {
    name:String,
    size:u32
}
