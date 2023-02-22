import sys
import re
import functools

def LineIntoCavePair(line: str):
    line_regex = "(\w+)-(\w+)"
    line_split = re.findall(line_regex, line)
    return (line_split[0][0], line_split[0][1])

def ParseInput():
    file_path = sys.argv[1]
    file = open(file_path, 'r')
    input_lines = file.read().splitlines()
    return list(map(lambda line: LineIntoCavePair(line), input_lines))

def CreateCaveMap(input: list[tuple[str,str]]):
    cave_map = {}
    for (start,end) in input:
        if not (start in cave_map):
            cave_map[start] = set()
        if not (end in cave_map):
            cave_map[end] = set()
        cave_map[start].add(end)
        cave_map[end].add(start)
    return cave_map

def TraverseCave(cave_map: dict[str, set[str]], small_visited: set[str], curr_cave: str, double_used: bool):   
    if curr_cave == "end":
        return 1
    if curr_cave == "start" and "start" in small_visited:
        return 0
    if curr_cave in small_visited:
        if not double_used:
            double_used = True
        else:
            return 0
    
    if re.match("([a-z]+)", curr_cave):
        small_visited.add(curr_cave)

    return functools.reduce(lambda acc, next: acc + TraverseCave(cave_map, small_visited.copy(), next, double_used), cave_map[curr_cave], 0)  

def Part1(input):
    cave_map = CreateCaveMap(input)
    return TraverseCave(cave_map, set(), "start", True)

def Part2(input):
    cave_map = CreateCaveMap(input)
    return TraverseCave(cave_map, set(), "start", False)

def main():
    input = ParseInput()
    part_1_result = Part1(input)
    print("Part 1 Result: {}".format(part_1_result)) # 3679
    part_2_result = Part2(input)
    print("Part 2 Result: {}".format(part_2_result)) # 107395

if __name__ == "__main__":
    main()