import sys
import functools
import queue
from enum import Enum

class StartEndEnum(Enum):
    Start = 'S'
    End = 'E'

def ParseInput(input_lines: list[str]):
    def MapPointToHeight(point):
        if point == StartEndEnum.Start.value:
            return ord('a')
        elif point == StartEndEnum.End.value:
            return ord('z')
        else:
            return ord(point)
    line_reduction = lambda line: functools.reduce(lambda acc, x: acc + [MapPointToHeight(x)], line, [])
    return functools.reduce(lambda acc, x: acc + [line_reduction(x)], input_lines, [])

def TraversalFuncList():
    up_traversal = lambda x: (x[0]-1,x[1])
    right_traversal = lambda x: (x[0],x[1]+1)
    down_traversal = lambda x: (x[0]+1,x[1])
    left_traversal = lambda x: (x[0],x[1]-1)
    return [up_traversal, right_traversal, down_traversal, left_traversal]

def GetCharPositions(input_lines, to_find):
    char_list = []
    for y, line in enumerate(input_lines):
        for x, char in enumerate(line):
            if char == to_find:
                char_list.append((y,x))
    return char_list

def GetMinimumSteps(input_grid, starting_points, starting_height, end_points):
    traversal_queue = queue.Queue()
    for point in starting_points:
        traversal_queue.put((point, starting_height, 0))
    visited_points = set()
    while(not traversal_queue.empty()):
        curr_tuple = traversal_queue.get()
        curr_point = curr_tuple[0]
        last_height = curr_tuple[1]
        curr_steps = curr_tuple[2]
        if curr_point in visited_points:
            continue
        (curr_y, curr_x) = curr_point
        if curr_y < 0 or curr_y >= len(input_grid) or curr_x < 0 or curr_x >= len(input_grid[curr_y]):
            continue
        curr_height = input_grid[curr_y][curr_x]
        if curr_height - last_height > 1:
            continue
        if curr_point in end_points:
            return curr_steps
        visited_points.add(curr_point)
        for traversal_func in TraversalFuncList():
            next_point = traversal_func(curr_point)      
            traversal_queue.put((next_point, curr_height, curr_steps + 1))

file_path = sys.argv[1]
file = open(file_path, 'r')
input_lines = file.read().splitlines()
mountain_grid = list(ParseInput(input_lines))

# Part 1 > 504
start_points = GetCharPositions(input_lines, StartEndEnum.Start.value)
end_points = GetCharPositions(input_lines, StartEndEnum.End.value)
part_1_result = GetMinimumSteps(mountain_grid, start_points, ord('a'), end_points)
print("Part 1 Result: {}".format(part_1_result))

# Part 2 > 500
start_points = GetCharPositions(input_lines, 'a')
end_points = GetCharPositions(input_lines, StartEndEnum.End.value)
part_2_result = GetMinimumSteps(mountain_grid, start_points, ord('a'), end_points)
print("Part 2 Result: {}".format(part_2_result))