import sys
from enum import Enum
from queue import Queue

class Facing(Enum):
    Up = '^'
    Right = '>'
    Down = 'v'
    Left = '<'

def IsFacing(char):
    facing_list = [facing.value for facing in Facing]
    return char in facing_list

class Space(Enum):
    Wall = '#'
    Open = '.'

class Blizzard:
    def __init__(self, start_point: tuple[int,int], facing: Facing):
        self.Point = start_point
        self.Facing = facing
    def Move(self, grid_map):
        next_pos = GetNextPos(grid_map, self.Point, self.Facing)
        (next_y, next_x) = next_pos
        while grid_map[next_y][next_x] != Space.Open:
            next_pos = GetNextPos(grid_map, next_pos, self.Facing)
        self.Point = next_pos

def GetNextPos(grid_map, curr_pos, facing):
    traversal_lambdas = {
        Facing.Up: lambda point: (point[0] - 1, point[1]) if point[0] != 0 else (len(grid_map) - 1, point[1]),
        Facing.Down: lambda point: (point[0] + 1, point[1]) if point[0] != len(grid_map) - 1 else (0, point[1]),
        Facing.Left: lambda point: (point[0], point[1] - 1) if point[1] != 0 else (point[0], len(grid_map[point[0]]) - 1),
        Facing.Right: lambda point: (point[0], point[1] + 1) if point[1] != len(grid_map[point[0]]) - 1 else (point[0], 0) 
    }
    traversal_function = traversal_lambdas[facing]
    curr_point = curr_pos
    while True:
        curr_point = traversal_function(curr_point)
        (curr_y, curr_x) = curr_point
        if grid_map[curr_y][curr_x] is Space.Open:
            return curr_point
        else:
            continue

def ParseGrid(input_lines):
    map_grid = []
    blizzards = []
    start = None
    end = None
    for (y, line) in enumerate(input_lines):
        next_line = []
        for (x, char) in enumerate(line):
            if y == 0 and char != Space.Wall.value:
                start = (y,x)
            elif y == len(input_lines) - 1 and char != Space.Wall.value:
                end = (y,x)
            if IsFacing(char):
                blizzards.append(Blizzard((y,x), Facing(char)))
            next_space = Space.Open if IsFacing(char) else Space(char)
            next_line.append(next_space)
        map_grid.append(next_line)
    return (map_grid, blizzards, start, end)

file_path = sys.argv[1]
file = open(file_path, 'r')
input_lines = file.read().splitlines()
(map_grid, blizzards, start_point, end_point) = ParseGrid(input_lines)

# Part 1
def GetNextBlizzard(map_grid, blizzards: list[Blizzard]):
    blizzard_set = set()
    for blizzard in blizzards:
        blizzard.Move(map_grid)
        blizzard_set.add(blizzard.Point)
    return (blizzard_set, blizzards)

def Part1(map_grid, blizzards, start_point, end_point):
    def InBounds(point, map_grid):
        (y_pos, x_pos) = point
        return ((y_pos >= 0 and x_pos >= 0) and
                (y_pos < len(map_grid) and x_pos < len(map_grid[y_pos])) and
                (map_grid[y_pos][x_pos]))
    traversal_lambdas = [
                lambda point: (point[0]+1, point[1]),
                lambda point: (point[0]-1, point[1]),
                lambda point: (point[0], point[1]+1),
                lambda point: (point[0], point[1]-1),
                lambda point: (point[0], point[1]),
    ]
    current_points = set([start_point])
    next_points = set()
    blizzard_point_set = set()
    curr_blizzards = blizzards
    num_steps = 0
    while True:
        # Increment the blizzards
        (blizzard_point_set, curr_blizzards) = GetNextBlizzard(map_grid, curr_blizzards)
        for point in current_points:
            if point == end_point: 
                return num_steps
            for curr_lambda in traversal_lambdas:
                next_point = curr_lambda(point)
                (next_y, next_x) = next_point
                if (next_point not in next_points and 
                    next_point not in blizzard_point_set and
                    InBounds(next_point, map_grid) and
                    map_grid[next_y][next_x] == Space.Open):
                    next_points.add(next_point)
        current_points = next_points
        next_points = set()
        num_steps += 1
        
part1_result = Part1(map_grid, blizzards, start_point, end_point)
pass