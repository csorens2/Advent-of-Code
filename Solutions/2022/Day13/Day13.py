import sys
from enum import Enum
import functools

class ParseLineReturn(Enum):
    Done_True = 1
    Done_False = 2
    Not_Done = 3 

def CompareList(first: list, second: list):
    for i in range(0, len(second)):
        if i >= len(first): 
            return ParseLineReturn.Done_True
        first_val = first[i]
        second_val = second[i]
        # Doing an int-int comparison
        if type(first_val) == int and type(second_val) == int:
            if first_val == second_val:
                continue
            elif first_val < second_val:
                return ParseLineReturn.Done_True
            else:
                return ParseLineReturn.Done_False
        # Doing a list-list comparison. Convert any if necessary
        if type(first_val) != list:
            first_val = [first_val]
        if type(second_val) != list:
            second_val = [second_val]
        rec_result = CompareList(first_val, second_val)
        if rec_result !=ParseLineReturn.Not_Done:
            return rec_result
    if len(first) > len(second):
        return ParseLineReturn.Done_False
    else:
        return ParseLineReturn.Not_Done

def RecParseLine(line_iter):
    parsed_list = []
    curr_num = str()
    for next_char in line_iter:
        # Finish the current number
        if (next_char == ']' or next_char == ',') and len(curr_num) > 0:
            curr_num = int(curr_num)
            parsed_list.append(curr_num)
            curr_num = str()
        # Recurse deeper
        if next_char == '[':
            (nested_list, updated_iter) = RecParseLine(line_iter)
            parsed_list.append(nested_list)
            line_iter = updated_iter
        # Return recursion
        if next_char == ']':
            return (parsed_list, line_iter)
        # Continue current number
        if next_char.isnumeric():
            curr_num += next_char
    return parsed_list[0]   

def ParseInput(input_lines):
    for line in input_lines:
        yield RecParseLine(iter(line))

file_path = sys.argv[1]
file = open(file_path, 'r')
input_lines = list(filter(lambda x: x != str(), file.read().splitlines()))
parsed_lines = list(ParseInput(input_lines))

# Part 1 > 6046
def Part1(input_lines):
    right_order_sum = 0
    for i in range(0, len(input_lines), 2):
        left_list = input_lines[i]
        right_list = input_lines[i+1]
        result = CompareList(left_list, right_list)
        if result != ParseLineReturn.Done_False:
            right_order_sum+=(i//2) + 1
    return right_order_sum

part_1_result = Part1(parsed_lines)
print("Part 1 Result: {}".format(part_1_result))

# Part 2 > 21423
def Part2(input_lines):
    def ParseLineReturnComparator(x):
        if x == ParseLineReturn.Done_True:
            return 1
        elif x == ParseLineReturn.Not_Done:
            return 0
        else: 
            return -1
    divider_packets = [[[2]], [[6]]]
    input_lines.append(divider_packets[0])
    input_lines.append(divider_packets[1])
    sorted_lines = sorted(input_lines, key=functools.cmp_to_key(lambda x, y: ParseLineReturnComparator(CompareList(x,y))))
    ascending_lines = list(reversed(sorted_lines))
    divider_product = 1
    for (i,line) in enumerate(ascending_lines):
        if line in divider_packets:
            divider_product*=(i+1)
    return divider_product

part_2_result = Part2(parsed_lines)
print("Part 2 Result: {}".format(part_2_result))