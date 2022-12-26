import sys
from enum import Enum
import re
import functools

class Space(Enum):
    Empty = ' '
    Floor = '.'
    Wall = '#'

class Operation(Enum):
    Move = 1
    Rotate = 2

class Orientation(Enum):
    Up = 0
    Right = 1
    Down = 2
    Left = 3

class Rotation(Enum):
    Right = 'R'
    Left = 'L'

class Person:
    def __init__(self, start_y: int, start_x: int, orientation: Orientation):
        self.Orientation = orientation
        self.Y = start_y
        self.X = start_x
    def Rotate(self, rotation: Rotation):
        shift = -1 if rotation == Rotation.Left else 1
        orientation_value = self.Orientation.value + shift
        if orientation_value == -1:
            self.Orientation = Orientation.Left
        elif orientation_value == 4:
            self.Orientation = Orientation.Up
        else:
            self.Orientation = Orientation(orientation_value)
    def Move(self, grid_map):
        (next_y, next_x) = GetNextPos(grid_map, (self.Y, self.X), self.Orientation)
        if grid_map[next_y][next_x] == Space.Floor:
            self.Y = next_y
            self.X = next_x

def GetNextPos(grid_map, curr_pos, orientation):
    traversal_lambdas = {
        Orientation.Up: lambda point: (point[0] - 1, point[1]) if point[0] != 0 else (len(grid_map) - 1, point[1]),
        Orientation.Down: lambda point: (point[0] + 1, point[1]) if point[0] != len(grid_map) - 1 else (0, point[1]),
        Orientation.Left: lambda point: (point[0], point[1] - 1) if point[1] != 0 else (point[0], len(grid_map[point[0]]) - 1),
        Orientation.Right: lambda point: (point[0], point[1] + 1) if point[1] != len(grid_map[point[0]]) - 1 else (point[0], 0) 
    }
    traversal_function = traversal_lambdas[orientation]
    curr_point = curr_pos
    while True:
        curr_point = traversal_function(curr_point)
        (curr_y, curr_x) = curr_point
        if grid_map[curr_y][curr_x] is not Space.Empty:
            return curr_point
        else:
            continue

def ParseGrid(map_lines):
    max_width = functools.reduce(lambda acc, next: max(acc, len(next)), map_lines, 0)
    map_grid = []
    for line in map_lines:
        next_line = [Space.Empty] * max_width
        for (i, char) in enumerate(line):
            next_line[i] = Space(char)
        map_grid.append(next_line)
    return map_grid

def ParseInput(lines):
    map_lines = []
    traversal_operations = []
    traversal_regex = "(\d+|[a-zA-Z])"
    for line in lines:
        if line == "":
            continue
        elif re.match(traversal_regex, line):
            matches = re.findall(traversal_regex, line)
            for match in matches:
                if str.isnumeric(match):
                    traversal_operations.append((Operation.Move, match))
                else:
                    traversal_operations.append((Operation.Rotate, match))
        else:
            map_lines.append(line)
    map_grid = ParseGrid(map_lines)
    return (map_grid, traversal_operations)

# Part 1
def PartOne(map_grid, traversal_operations):
    def GetInitialPos(map_grid):
        for y in range(0, len(map_grid)):
            for x in range(0, len(map_grid[y])):
                if map_grid[y][x] == Space.Floor:
                    return(y,x)
    (y_pos, x_pos) = GetInitialPos(map_grid)
    person = Person(y_pos, x_pos, Orientation.Right)
    for traversal in traversal_operations:
        #print("Start: ({},{}) Orient: {}".format(person.Y, person.X, person.Orientation))
        #print("")
        (operation, value) = traversal
        if operation == Operation.Move:
            num = int(value)
            for _ in range(0, num):
                person.Move(map_grid)
        if operation == Operation.Rotate:
            person.Rotate(Rotation(value))
    orientation_points_dict = {
        Orientation.Right: 0,
        Orientation.Down: 1,
        Orientation.Left: 2,
        Orientation.Up: 3
    }
    return (1000*(person.Y+1)) + (4*(person.X+1)) + orientation_points_dict[person.Orientation]

def main():
    file_path = sys.argv[1]
    file = open(file_path, 'r')
    input_lines = file.read().splitlines()
    (map_grid, traversal_operations) = ParseInput(input_lines)
    part_1_result = PartOne(map_grid, traversal_operations)

if __name__ == "__main__":
    main()