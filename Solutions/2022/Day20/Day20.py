import sys
import re
import functools
from queue import Queue

class NumberNode:
    def __init__(self, value):
        self.Value = value
        self.Switched = False
        self.Prev = None
        self.Next = None
    def ShiftBack(self):
        prev_node = self.Prev
        new_prev_node = self.Prev.Prev
        
    def ShiftNode(self):
        shift_step = None
        if self.Value < 0:
            shift_step = -1
        else:
            shift_step = 1
        for _ in range(0, self.Value, shift_step):


def ParseInput(lines):
    root_node = None
    last_node = None
    curr_node = None
    for line in lines:
        curr_node = NumberNode(line)
        if root_node == None:
            root_node = curr_node
        if last_node == None:
            last_node = curr_node
            continue
        last_node.Next = curr_node
        curr_node.Prev = last_node  
        last_node = curr_node
    curr_node.Next = root_node
    root_node.Prev = curr_node
    return root_node

file_path = sys.argv[1]
file = open(file_path, 'r')
input_lines = file.read().splitlines()
root_node = ParseInput(input_lines)

def Part1


test = 1