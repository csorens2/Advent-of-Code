import sys
import re
import copy

def ParseInput(input_lines):
    point_regex = "(\d+),(\d+)"
    for line in input_lines:
        matches = re.findall(point_regex, line)
        match_tuples = []
        for match in matches:
            match_tuples.append((int(match[0]), int(match[1])))
        yield match_tuples

def GetRockSet(tuple_list):
    rock_set = set()
    step_lamda = lambda first, second: 1 if first < second else -1
    inclusive_range_lambda = lambda start, end, step: range(start, end + 1, step) if step == 1 else range(start, end - 1, step)
    for tuple_line in tuple_list:
        for i in range(0, len(tuple_line) - 1):
            start_point = tuple_line[i]
            end_point = tuple_line[i+1]
            (start_x, start_y) = start_point
            (end_x, end_y) = end_point
            if start_x != end_x:
                step = step_lamda(start_x, end_x)
                for i in inclusive_range_lambda(start_x, end_x, step):
                    rock_set.add((i, start_y))
            if start_y != end_y:
                step = step_lamda(start_y, end_y)
                for i in inclusive_range_lambda(start_y, end_y, step):
                    rock_set.add((start_x, i))
    return rock_set

def FindYMax(tuple_list):
    y_max = -1
    for tuple_line in tuple_list:
        for tuple in tuple_line:
            y_max = max(y_max, tuple[1])
    return y_max

def DropSand(rock_sand_set, sand_pos, y_max):
    (sand_x, sand_y) = sand_pos
    if sand_y == y_max:
        rock_sand_set.add((sand_x, sand_y))
        return (rock_sand_set, True)
    if (sand_x, sand_y + 1) not in rock_sand_set:
        return DropSand(rock_sand_set, (sand_x, sand_y + 1), y_max)
    elif (sand_x - 1, sand_y + 1) not in rock_sand_set:
        return DropSand(rock_sand_set, (sand_x - 1, sand_y + 1), y_max)
    elif (sand_x + 1, sand_y + 1) not in rock_sand_set:
        return DropSand(rock_sand_set, (sand_x + 1, sand_y + 1), y_max)
    else:
        rock_sand_set.add((sand_x, sand_y))
        return (rock_sand_set, False)

file_path = sys.argv[1]
file = open(file_path, 'r')
input_lines = file.read().splitlines()
parsed_lines = list(ParseInput(input_lines))
rock_set = GetRockSet(parsed_lines)
y_max = FindYMax(parsed_lines)

# Part 1 > 843
def PourSandUntilAbyss(rock_set, y_max):
    sand_count = 0
    rock_with_sand = rock_set
    while True:
        next_sand = (500, 0)
        (rock_with_sand, finished) = DropSand(rock_with_sand, next_sand, y_max)
        if finished:
            return sand_count
        else:
            sand_count+=1
part_1_result = PourSandUntilAbyss(copy.deepcopy(rock_set), y_max)
print("Part 1 result: {}".format(part_1_result))

# Part 2 > 27625
def PourSandUntilCoverup(rock_set, y_max):
    sand_count = 0
    rock_with_sand = rock_set
    while True:
        next_sand = (500, 0)
        sand_count += 1
        (rock_with_sand, _) = DropSand(rock_with_sand, next_sand, y_max)
        if next_sand in rock_with_sand:
            return sand_count
floor_height = y_max + 1
part_2_result = PourSandUntilCoverup(copy.deepcopy(rock_set), floor_height)
print("Part 2 result: {}".format(part_2_result))