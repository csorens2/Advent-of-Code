import sys
import re

class Monkey:
    def __init__(self, name:str, value:int = None):
        self.Name = name
        self.Value = value
        self.Monkey1Name = None
        self.Monkey1Value = None
        self.Monkey2Name = None
        self.Monkey2Value = None
        self.Operation = None
        self.DependentMonkeys = []
    def AssignValue(self, value):
        self.Value = value
        for dependent_monkey in self.DependentMonkeys:
            dependent_monkey.AssignValueByName(self.Value, self.Name)
    def AssignValueByName(self, value, monkeyname):
        if self.Monkey1Name == monkeyname:
            self.Monkey1Value = value
        else:
            self.Monkey2Value = value
        if self.Monkey1Value != None and self.Monkey2Value != None:
            self.AssignValue(int(self.Operation(self.Monkey1Value, self.Monkey2Value)))
    def AddDependentMonkey(self, dependent_monkey):
        self.DependentMonkeys.append(dependent_monkey)  
    def HasHooted(self):
        return True if self.Value != None else False      

file_path = sys.argv[1]
file = open(file_path, 'r')
input_lines = file.read().splitlines()

# Part 1
def Part1(lines):
    operation_lambdas = {
        '+': lambda x,y: x + y,
        '-': lambda x,y: x - y,
        '*': lambda x,y: x * y,
        '/': lambda x,y: x / y
    }
    monkey_dict = {}
    for line in lines:
        monkey_combo_regex = "(\w+): (\w+) ([\+\-\*\/]) (\w+)"
        if re.match(monkey_combo_regex, line):
            (name, monkey1, operation, monkey2) = re.findall(monkey_combo_regex, line)[0]
            combo_monkey = monkey_dict[name] if name in monkey_dict else Monkey(name)
            combo_monkey.Operation = operation_lambdas[operation]
            combo_monkey.Monkey1Name = monkey1
            combo_monkey.Monkey2Name = monkey2
            monkey_dict[name] = combo_monkey
            for input_monkey_name in [monkey1, monkey2]:
                input_monkey = None
                if input_monkey_name not in monkey_dict:
                    input_monkey = Monkey(input_monkey_name)
                    monkey_dict[input_monkey_name] = input_monkey
                else:
                    input_monkey = monkey_dict[input_monkey_name]
                if input_monkey.Value == None:
                    input_monkey.AddDependentMonkey(combo_monkey)
                else:
                    combo_monkey.AssignValueByName(input_monkey.Value, input_monkey_name) 
        single_monkey_regex = "(\w+): (\d+)"
        if re.match(single_monkey_regex, line):
            (name, value) = re.findall(single_monkey_regex, line)[0]
            if name in monkey_dict:
                monkey_dict[name].AssignValue(int(value))
            else:
                monkey_dict[name] = Monkey(name)
                monkey_dict[name].AssignValue(int(value))
        if "root" in monkey_dict and monkey_dict["root"].HasHooted():
            return monkey_dict["root"].Value
    
part_1_result = Part1(input_lines)
print("Part 1 Result {}".format(part_1_result))