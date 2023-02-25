import sys
import re
import functools
from enum import Enum

def LineIntoDotPair(line: str):
    line_regex = "(\w+)-(\w+)"
    line_split = re.findall(line_regex, line)
    return (line_split[0][0], line_split[0][1])

def ParseInput():
    file_path = sys.argv[1]
    file = open(file_path, 'r')
    input_lines = file.read().splitlines()

    point_list = []
    points_regex = re.compile("(\d+),(\d+)")
    input_lines_iter = iter(input_lines)
    line = next(input_lines_iter)
    while points_regex.match(line):
        line_split = points_regex.findall(line)
        point_list.append((int(line_split[0][0]), int(line_split[0][1])))
        line = next(input_lines_iter)

    fold_list = []
    fold_regex = re.compile(".*([xy])=(\d+)")
    for line in input_lines_iter:
        line_split = fold_regex.findall(line)
        fold_list.append((line_split[0][0], int(line_split[0][1])))   
    return (point_list, fold_list)

def VertFold(point_set:set, next_point, y_line):
    (next_x, next_y) = next_point
    if next_y < y_line:
        point_set.add(next_point)
    else:
        line_delta = next_y - y_line
        new_point = (next_x, y_line - line_delta)
        point_set.add(new_point)
    return point_set

def HorizFold(point_set:set, next_point, x_line):
    (next_x, next_y) = next_point
    if next_x < x_line:
        point_set.add(next_point)
    else:
        line_delta = next_x - x_line
        new_point = (x_line - line_delta, next_y)
        point_set.add(new_point)
    return point_set

def PerformFolds(input):
    (point_list, fold_list) = input
    point_set = set(point_list)
    first_fold_dots = None
    for (fold_dir, fold_val) in fold_list:
        fold_func = None
        if fold_dir == "x":
            fold_func = HorizFold
        else:
            fold_func = VertFold
        point_set = functools.reduce(lambda acc, next: fold_func(acc, next, fold_val), point_set, set())
        if first_fold_dots == None:
            first_fold_dots = len(point_set)
    PrintPoints(point_set)
    return first_fold_dots

def PrintPoints(points):
    max_x = max(points, key=lambda i: i[0])[0]
    max_y = max(points, key=lambda i: i[1])[1]
    for y in range(max_y + 1):
        for x in range(max_x + 1):
            if (x,y) in points:
                print("#", end=" ")
            else:
                print(".", end=" ")
        print()

def main():
    input = ParseInput()
    part_1_result = PerformFolds(input)
    print("Part 1 Result: {}".format(part_1_result)) # 708
    part_2_result = "EBLUBRFH"
    print("Part 2 Result: {}".format(part_2_result)) # EBLUBRFH

if __name__ == "__main__":
    main()