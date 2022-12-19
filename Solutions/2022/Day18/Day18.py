import sys
import re
import functools
from queue import Queue

def ParseInput(lines):
    for line in lines:
        num_regex = "(\d+)"
        matches = re.findall(num_regex, line)
        yield (int(matches[0]), int(matches[1]), int(matches[2]))


file_path = sys.argv[1]
file = open(file_path, 'r')
input_lines = file.read().splitlines()
cube_list = list(ParseInput(input_lines))

def GetCubeLambdas():
    return [
        lambda cube: (cube[0]+1, cube[1], cube[2]),
        lambda cube: (cube[0]-1, cube[1], cube[2]),
        lambda cube: (cube[0], cube[1]+1, cube[2]),
        lambda cube: (cube[0], cube[1]-1, cube[2]),
        lambda cube: (cube[0], cube[1], cube[2]+1),
        lambda cube: (cube[0], cube[1], cube[2]-1)
    ]

# Part 1 > 3454
def Part1(input_set: set[tuple[int,int,int]]):
    side_count = 0
    for cube in input_set:
        side_count += functools.reduce(lambda acc, next_func: acc + 1 if next_func(cube) not in input_set else acc, GetCubeLambdas(), 0)
    return side_count

part_1_result = Part1(cube_list)
print("Part 1 Result: {}".format(part_1_result))

# Part 2 
def GetWaterBounds(cube_list):
    max_x = max(cube_list, key=lambda cube: cube[0])[0]
    min_x = min(cube_list, key=lambda cube: cube[0])[0]
    max_y = max(cube_list, key=lambda cube: cube[1])[1]
    min_y = min(cube_list, key=lambda cube: cube[1])[1]
    max_z = max(cube_list, key=lambda cube: cube[2])[2]
    min_z = min(cube_list, key=lambda cube: cube[2])[2]
    return (max_x+1, min_x-1, max_y+1, min_y-1, max_z+1, min_z-1)

def GetWaterCubes(input_cubes):
    water_set = set()
    (max_x, min_x, max_y, min_y, max_z, min_z) = GetWaterBounds(input_cubes)
    water_queue = Queue()
    water_queue.put((max_x, max_y, max_z))
    while not water_queue.empty():
        next_cube = water_queue.get()
        (next_cube_x, next_cube_y, next_cube_z) = next_cube
        if (next_cube_x < min_x or next_cube_y < min_y or next_cube_z < min_z or
            next_cube_x > max_x or next_cube_y > max_y or next_cube_z > max_z):
            continue
        if next_cube in input_cubes or next_cube in water_set:
            continue
        water_set.add(next_cube)
        for cube_lambda in GetCubeLambdas():
            water_queue.put(cube_lambda(next_cube))
    return water_set

def DropletSurfaceArea(input_cubes):
    water_cubes = GetWaterCubes(input_cubes)
    side_count = 0
    for cube in input_cubes:
        side_count += functools.reduce(lambda acc, next_func: acc + 1 if next_func(cube) in water_cubes else acc, GetCubeLambdas(), 0)
    return side_count

part_2_result = DropletSurfaceArea(cube_list)
print("Part 2 Result: {}".format(part_2_result))