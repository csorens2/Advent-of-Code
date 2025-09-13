import sys
import re
from functools import partial
import math
import functools
import copy

class Monkey:
    def __init__(self):
        self.Number = None
        self.Items = []
        self.Operation_Func = None
        self.Test_Num = None
        self.True_Target = None
        self.False_Target = None

def ParseInput(input_lines: list[str]):
    num_regex = "Monkey (\d*):"
    starting_items_regex = "(?:  Starting items:)?(?:[\s,]+)(\d+)"
    operation_field_regex = "(?:  Operation: new = old )([*+])\s(\d+|old)"
    OperationDict = {
        "*": lambda x,y: x * y,
        "+": lambda x,y: x + y
    }
    test_regex = "(?:  Test: divisible by )(\d+)"
    true_regex = "(?:    If true: throw to monkey )(\d+)"
    false_regex = "(?:    If false: throw to monkey )(\d+)"
    monkey_list = []
    next_monkey = Monkey()
    for line in input_lines:
        if re.match(num_regex, line):
            next_monkey.Number = int(re.search(num_regex, line).groups()[0])
        elif re.match(starting_items_regex, line):
            matches = re.findall(starting_items_regex, line)
            for match in matches:
                next_monkey.Items.append(int(match))
        elif re.match(operation_field_regex, line):
            matches = re.findall(operation_field_regex, line)
            next_operation = matches[0][0]
            next_func = OperationDict[next_operation]
            if matches[0][1] == "old":
                test_func = lambda func,x: func(x,x)
                next_monkey.Operation_Func = partial(test_func, next_func)
            else:
                next_monkey.Operation_Func = partial(next_func, int(matches[0][1]))
        elif re.match(test_regex, line):
            test_num = int(re.findall(test_regex, line)[0])
            next_monkey.Test_Num = test_num
        elif re.match(true_regex, line):
            true_num = int(re.findall(true_regex, line)[0])
            next_monkey.True_Target = true_num
        elif re.match(false_regex, line):
            false_num = int(re.findall(false_regex, line)[0])
            next_monkey.False_Target = false_num
        else:
            monkey_list.append(next_monkey)
            next_monkey = Monkey()
            continue
    monkey_list.append(next_monkey)
    return monkey_list

def GetMonkeyBusiness(monkey_list: list[Monkey], round_count, modulo: bool):
    inspection_count = [0] * len(monkey_list)
    monkey_lcm = functools.reduce(lambda acc, x: math.lcm(acc, x.Test_Num), monkey_list, 1)
    for _ in range(0, round_count):
        for monkey in monkey_list:
            monkey_num = monkey.Number
            for item in monkey.Items:
                inspection_count[monkey_num] += 1
                item_worry = None
                if modulo:
                    item_worry = monkey.Operation_Func(item) % monkey_lcm
                else:
                    item_worry = monkey.Operation_Func(item) // 3
                target_monkey = None
                if item_worry % monkey.Test_Num == 0:
                    target_monkey = monkey.True_Target
                else:
                    target_monkey = monkey.False_Target
                monkey_list[target_monkey].Items.append(item_worry)
            monkey.Items = []
    inspection_count.sort(reverse=True)
    top_monkeys = inspection_count[:2]
    return math.prod(top_monkeys)

file_path = sys.argv[1]
file = open(file_path, 'r')
input_lines = file.read().splitlines()
initial_monkeys = list(ParseInput(input_lines))

# Part 1 > 102399
part_1_result = GetMonkeyBusiness(copy.deepcopy(initial_monkeys), 20, False)
print("Part 1 Result: {}".format(part_1_result))

# Part 2 > 23641658401
part_2_result = GetMonkeyBusiness(copy.deepcopy(initial_monkeys), 10000, True)
print("Part 2 Result: {}".format(part_2_result))