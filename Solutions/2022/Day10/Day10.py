import sys
from enum import Enum

class Instruction(Enum):
    addx = "addx"
    noop= "noop"

def IterateInstructions(input_lines):
    for line in input_lines:
        split_line = line.split()
        next_instruction = Instruction(split_line[0])
        match next_instruction:
            case Instruction.noop:
                yield (Instruction.noop, 0)
            case Instruction.addx:
                yield (Instruction.noop, 0)
                yield (Instruction.addx, int(split_line[1]))

file_path = sys.argv[1]
file = open(file_path, 'r')
input_lines = file.read().splitlines()

# Part 1 > 14860
def Part1(input_lines, cycle_list):
    register_value = 1
    signal_sum = 0
    cycle = 1
    for(instruction, value) in IterateInstructions(input_lines):
        if cycle in cycle_list:
            signal_power = (cycle) * register_value
            signal_sum += signal_power
        if instruction == Instruction.addx:
            register_value += value
        cycle+=1
    return signal_sum
            
part_1_cycle_list = [20, 60, 100, 140, 180, 220]
part_1_result = Part1(input_lines, part_1_cycle_list)
print("Part 1 Result: {}".format(part_1_result))

# Part 2 > RGZEHURK
def Part2(input_lines, end_of_line_cycle):
    register_value = 1
    cycle = 0
    sprite_in_range = lambda cycle, sprite: (sprite - 1) <= cycle and cycle <= (sprite + 1)
    for(instruction, value) in IterateInstructions(input_lines):
        if (register_value - 1) <= cycle and cycle <= (register_value + 1):
            print("#", end='')
        else:
            print(".", end='')
        if cycle == end_of_line_cycle:
            print("")
            cycle = 0
        else:
            cycle+=1
        if instruction == Instruction.addx:
            register_value += value
        
end_of_line_cycle = 39
Part2(input_lines, end_of_line_cycle)