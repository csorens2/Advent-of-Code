import { readFileSync } from 'fs';

function get_input(filepath) {
    return readFileSync(filepath, 'utf-8').split(",").map(val => parseInt(val));
}

function process_instructions(instructions: number[], i: number)
{
    const next_instruction = instructions[i];
    if (next_instruction == 99)
    {
        return instructions;
    }
    else 
    {
        const operation_dict = 
        {
            1 : (x, y) => x + y,
            2 : (x, y) => x * y
        };
        const x = instructions[instructions[i+1]];
        const y = instructions[instructions[i+2]];
        instructions[instructions[i+3]] = operation_dict[next_instruction](x,y);
        return process_instructions(instructions, i + 4);
    }
}

function part_one(input: number[]) 
{
    input[1] = 12;
    input[2] = 2;
    return process_instructions(input, 0)[0];
}

function part_two(input: number[])
{
    for(let i = 99; i >= 0; i--)
    {
        for(let j = 99; j >= 0; j--)
        {
            let input_copy = [...input];
            input_copy[1] = i;
            input_copy[2] = j;
            const result = process_instructions(input_copy, 0);
            if (result[0] == 19690720)
            {
                return (100*i) + j;
            }
        }
    }

    throw new Error("Did not find expected result.");
}

const input_file_path = __dirname + "/../input.txt";
const input = get_input(input_file_path);

const part_one_result = part_one([...input]);
console.log(`Part 1 Result: ${part_one_result}`); // 3760627
const part_two_result = part_two([...input]);
console.log(`Part 2 Result: ${part_two_result}`); // 7195