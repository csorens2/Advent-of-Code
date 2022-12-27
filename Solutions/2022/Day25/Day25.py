import sys
from enum import Enum

def SnafuToDigit(char):
    snafu_map = {
        '2': 2,
        '1': 1,
        '0': 0,
        '-': -1,
        '=': -2
    }
    return snafu_map[char]
 

def ParseInput(lines):
    for line in lines:



file_path = sys.argv[1]
file = open(file_path, 'r')
input_lines = file.read().splitlines()
(map_grid, blizzards, start_point, end_point) = ParseGrid(input_lines)