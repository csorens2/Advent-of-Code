import sys
import functools
import copy

class Elf:
    def __init__(self, point):
        self.Point = point
    def SetTarget(self, point):
        self.Target = point
    def Move(self, space_is_open):
        if space_is_open:
            self.Point = self.Target
        else:
            pass

def ParseInput(lines):
    elf_set = set()
    for (y, line) in enumerate(lines):
        for (x, char) in enumerate(line):
            if char == '#':
                elf_set.add(Elf((y,x)))    
    return elf_set

def UpPair():
    check = lambda point, set: (
        ((point[0] - 1, point[1]) not in set) and
        ((point[0] - 1, point[1] - 1) not in set) and 
        ((point[0] - 1, point[1] + 1) not in set))
    move = lambda point: (point[0] - 1, point[1])
    return (check, move)

def DownPair():
    check = lambda point, set: (
        ((point[0] + 1, point[1]) not in set) and
        ((point[0] + 1, point[1] - 1) not in set) and 
        ((point[0] + 1, point[1] + 1) not in set))
    move = lambda point: (point[0] + 1, point[1])
    return (check, move)

def LeftPair():
    check = lambda point, set: (
        ((point[0], point[1] - 1) not in set) and
        ((point[0] - 1, point[1] - 1) not in set) and 
        ((point[0] + 1, point[1] - 1) not in set))
    move = lambda point: (point[0], point[1] - 1)
    return (check, move)

def RightPair():
    check = lambda point, set: (
        ((point[0], point[1] + 1) not in set) and
        ((point[0] - 1, point[1] + 1) not in set) and 
        ((point[0] + 1, point[1] + 1) not in set))
    move = lambda point: (point[0], point[1] + 1)
    return (check, move)

def IsSolo(point, elf_set):
    pairs = [
        UpPair(),
        RightPair(),
        DownPair(),
        LeftPair()
    ]
    pair_checks = map(lambda x: x[0], pairs)
    return functools.reduce(lambda acc, next_func: acc and next_func(point, elf_set), pair_checks, True)

file_path = sys.argv[1]
file = open(file_path, 'r')
input_lines = file.read().splitlines()
elf_set = ParseInput(input_lines)

def Part1(elf_set: set[Elf], rounds):
    target_dict = {}
    func_list = [
        UpPair(),
        DownPair(),
        LeftPair(),
        RightPair()
    ]
    for _ in range(0, rounds):
        PrintElfSet(elf_set)
        print()
        del target_dict
        target_dict = {}
        elf_set_points = set(map(lambda x: x.Point, elf_set))
        for elf in elf_set:
            elf_point = elf.Point
            if IsSolo(elf_point, elf_set_points):
                elf.Target = elf_point
                target_dict[elf.Target] = True
                continue
            for (i, funcs) in enumerate(func_list):
                (check, move) = funcs
                if check(elf.Point, elf_set_points):
                    elf.Target = move(elf.Point)
                    if elf.Target in target_dict:
                        target_dict[elf.Target] = False
                    else:
                        target_dict[elf.Target] = True
                    break
                if i == 3:
                    elf.Target = elf_point
                    target_dict[elf.Target] = True
        for elf in elf_set:
            elf.Move(target_dict[elf.Target])
        func_list.append(func_list[0])
        func_list.pop(0)
        pass
    PrintElfSet(elf_set)
    x_min = functools.reduce(lambda acc, next: min(acc, next.Point[1]), elf_set, sys.maxsize)
    x_max = functools.reduce(lambda acc, next: max(acc, next.Point[1]), elf_set, -sys.maxsize)
    pass

    return elf_set

def PrintElfSet(elf_set):
    elf_set_map = set(map(lambda x: x.Point, elf_set))
    for y in range(0,13):
        for x in range(0,16):
            if (y,x) in elf_set_map:
                print("#", end='')
            else:
                print(".", end='')
        print()    

part_1_result = Part1(copy.deepcopy(elf_set), 10)
pass