import { readFileSync } from 'fs';

function get_input(filepath) {
    return readFileSync(filepath, 'utf-8');
}

function part_one() 
{
}

function part_two()
{
}

const input_file_path = __dirname + "/../input.txt";
const input = get_input(input_file_path);

//const part_one_result = part_one([...input]);
//console.log(`Part 1 Result: ${part_one_result}`);
//const part_two_result = part_two();
//console.log(`Part 2 Result: ${part_two_result}`);