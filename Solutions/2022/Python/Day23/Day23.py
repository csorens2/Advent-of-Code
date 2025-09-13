import sys
import functools
import copy
import itertools

class Elf:
    def __init__(self, point):
        self.Point = point
        self.Target = None
    def Move(self):
        self.Point = self.Target
        self.Target = None

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
    checks = map(lambda x: x[0], pairs)
    for check in checks:
        if not check(point, elf_set):
            return False
    return True

def SimulateElves(elf_set: set[Elf], round_iter):
    target_dict = {}
    first_no_movement_round = -1
    func_list = [
        UpPair(),
        DownPair(),
        LeftPair(),
        RightPair()
    ]
    for round in round_iter:
        del target_dict
        target_dict = {}
        elf_set_points = set(map(lambda x: x.Point, elf_set))
        for elf in elf_set:
            elf_point = elf.Point
            if IsSolo(elf_point, elf_set_points):
                continue
            for (check, move) in func_list:
                if check(elf.Point, elf_set_points):
                    elf.Target = move(elf.Point)
                    if elf.Target in target_dict:
                        target_dict[elf.Target] = False
                    else:
                        target_dict[elf.Target] = True
                    break
        no_movement = True
        for elf in elf_set:
            if elf.Target == None:
                continue
            if target_dict[elf.Target]:
                no_movement = False
                elf.Move()
            elf.Target = None
        if no_movement and first_no_movement_round == -1:
            first_no_movement_round = round
            break
        func_list.append(func_list[0])
        func_list.pop(0)
    x_min = functools.reduce(lambda acc, next: min(acc, next.Point[1]), elf_set, sys.maxsize)
    x_max = functools.reduce(lambda acc, next: max(acc, next.Point[1]), elf_set, -sys.maxsize)
    y_min = functools.reduce(lambda acc, next: min(acc, next.Point[0]), elf_set, sys.maxsize)
    y_max = functools.reduce(lambda acc, next: max(acc, next.Point[0]), elf_set, -sys.maxsize)
    total_spaces = (abs(x_max - x_min)+1) * (abs(y_max - y_min)+1)
    open_spaces = total_spaces - len(elf_set)
    return (open_spaces, first_no_movement_round)

def ParseInput(lines):
    elf_set = set()
    for (y, line) in enumerate(lines):
        for (x, char) in enumerate(line):
            if char == '#':
                elf_set.add(Elf((y,x)))    
    return elf_set

file_path = sys.argv[1]
file = open(file_path, 'r')
input_lines = file.read().splitlines()
elf_set = ParseInput(input_lines)   

# Part 1 > 3882
(part_1_result,_) = SimulateElves(copy.deepcopy(elf_set), range(1, 11))
print("Part 1 Result: {}".format(part_1_result))

# Part 2 > 1116
(_, part_2_result) = SimulateElves(copy.deepcopy(elf_set), itertools.count(1))
print("Part 2 Result: {}".format(part_2_result))