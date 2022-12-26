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
        while next_pos == Space.Wall:
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
    queue = Queue()
    queue.put((start_point, 0))
    last_blizard_movement = 0
    blizzard_point_set = set()
    curr_blizzards = blizzards
    while True:
        (curr_point, curr_steps) = queue.get()
        (curr_y, curr_x) = curr_point
        if start_point == end_point:
            return curr_steps
        if curr_steps > last_blizard_movement:
            (blizzard_point_set, curr_blizzards) = GetNextBlizzard(map_grid, curr_blizzards)
        if curr_point in blizzard_point_set:
            continue
        if not InBounds(curr_point, map_grid):
            continue
        if map_grid[curr_y][curr_x] == Space.Wall:
            continue
        traversal_lambdas = [
            lambda point: (point[0]+1, point[1]),
            lambda point: (point[0]-1, point[1]),
            lambda point: (point[0], point[1]+1),
            lambda point: (point[0], point[1]-1),
        ]
        for traversal in traversal_lambdas:
            queue.put((traversal(curr_point), curr_steps+1))
        
part1_result = Part1(map_grid, blizzards, start_point, end_point)
pass