import sys
import functools
from enum import Enum

class Operation(Enum):
    Up = 'U'
    Down = 'D'
    Left = 'L'
    Right = 'R'

MotionDict = {
    Operation.Up: lambda head: (head[0], head[1]+1),
    Operation.Down: lambda head: (head[0], head[1]-1),
    Operation.Left: lambda head: (head[0]-1, head[1]),
    Operation.Right: lambda head: (head[0]+1, head[1]),
}

def ParseInput(file_path):
    file = open(file_path, 'r')
    input_list = file.read().splitlines()
    input_list_split = map(lambda line: line.split(), input_list)
    input_list_tuple = map(lambda split: (Operation(split[0]), int(split[1])) , input_list_split)
    return input_list_tuple

def InRange(curr:tuple[int,int], next:tuple[int,int]):
    delta_x = abs(curr[0] - next[0])
    delta_y = abs(curr[1] - next[1])
    return delta_x <= 1 and delta_y <= 1

def GetNextPos(curr:tuple[int,int], next:tuple[int,int]):
    if InRange(curr, next):
        return next
    delta_x = next[0] - curr[0]
    delta_y = next[1] - curr[1]
    delta_list = [abs(delta_x), abs(delta_y)]
    # "Knight" Diag: Delta of 2 and 1 in any combo of directions
    # Regular Diag: Delta of two 2s in any combo of directions
    if (2 in delta_list and 1 in delta_list) or (sum(d == 2 for d in delta_list) == 2):
        # Each of x and y will be shifted by one. The direction of the shift is inverse of the delta
        transform_func = lambda x: -1 if x > 0 else 1 
        return (next[0] + transform_func(delta_x), next[1] + transform_func(delta_y))
    # Cardinal: Delta of 2 and 0 in any combo of directions
    elif 2 in delta_list and 0 in delta_list:
        # The shift will be the 1 in the inverse direction of the delta
        transform_func = lambda x: x if x == 0 else x // -2
        return (next[0] + transform_func(delta_x), next[1] + transform_func(delta_y))
    else:
        raise Exception("Should not hit this in GetNextTail")

def MoveKnots(operation_list, knot_list):
    tail_visited_locations = set([(0,0)])
    for operation in operation_list:
        motion_func = MotionDict[operation[0]]
        motion_iter = operation[1]
        for _ in range(0, motion_iter):
            knot_list[0] = motion_func(knot_list[0])
            for i in range(1, len(knot_list)):
                knot_list[i] = GetNextPos(knot_list[i-1], knot_list[i])
            tail_visited_locations.add(knot_list[len(knot_list)-1])
    return len(tail_visited_locations)

file_path = sys.argv[1]
parsed_input = list(ParseInput(file_path))

# Part 1 > 6190
part_1_knot_list = [(0,0)] * 2
part_1_result = MoveKnots(parsed_input, part_1_knot_list)
print("Part 1 result: {}".format(part_1_result))

# Part 2 > 2516
part_2_knot_list = [(0,0)] * 10
part_2_result = MoveKnots(parsed_input, part_2_knot_list)
print("Part 2 result: {}".format(part_2_result))