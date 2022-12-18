import sys
from enum import Enum

class JetDirection(Enum):
    Right = '>'
    Left = '<'

class Rock:
    def __init__(self, pieces: list[tuple[int,int]], starting_row: int):
        self.Pieces = list(map(lambda tuple: (tuple[0], tuple[1] + starting_row + 4), pieces))
    def CanDrop(self, previous_blocks: set[(int,int)]):
        for piece in self.Pieces:
            x_pos = piece[0]
            y_pos = piece[1]
            if (x_pos, y_pos - 1) in previous_blocks:
                return False
        return True
    def Drop(self):
        self.Pieces = list(map(lambda tuple: (tuple[0], tuple[1] - 1), self.Pieces))
    def CanShift(self, direction: JetDirection, previous_blocks: set[(int,int)]):
        for piece in self.Pieces:
            x_pos = piece[0]
            y_pos = piece[1]
            if direction == JetDirection.Right and ((x_pos + 1, y_pos) in previous_blocks or x_pos == 6):
                return False
            if direction == JetDirection.Left and ((x_pos - 1, y_pos) in previous_blocks or x_pos == 0):
                return False
        return True
    def Shift(self, direction: JetDirection):
        if direction == JetDirection.Right:
            self.Pieces = list(map(lambda tuple: (tuple[0] + 1, tuple[1]), self.Pieces))
        else:
            self.Pieces = list(map(lambda tuple: (tuple[0] - 1, tuple[1]), self.Pieces))

def GetRockPieces():
    return [
        [(2,0), (3,0), (4,0), (5,0)],
        [(3,0), (2,1), (3,1), (4,1), (3,2)],
        [(2,0), (3,0), (4,0), (4,1), (4,2)],
        [(2,0), (2,1), (2,2), (2,3)],
        [(2,0), (3,0), (2,1), (3,1)]
    ]  

def GetJetEnumerable(line):
    while True:
        for char in line:
            yield JetDirection(char)

def GetRockPiecesEnumerable():
    while True:
        for rock in GetRockPieces():
            yield rock

file_path = sys.argv[1]
file = open(file_path, 'r')
input_line = file.read()

def Part1(input_line, total_rocks):
    rock_count = 0
    height_list = [0] * 7
    pieces_set = set()
    for x in range(0, len(height_list)):
        pieces_set.add((x,0))
    jet_enumerable = GetJetEnumerable(input_line)
    for rock_pieces in GetRockPiecesEnumerable():
        if rock_count == total_rocks:
            return height_list
        next_rock = Rock(rock_pieces, max(height_list))
        # We push once before we loop. Its guaranteed to always work
        next_jet = next(jet_enumerable)
        next_rock.Shift(next_jet)            
        while(next_rock.CanDrop(pieces_set)):
            next_rock.Drop()
            next_jet = next(jet_enumerable)
            if next_rock.CanShift(next_jet, pieces_set):
                next_rock.Shift(next_jet)
        # Update the set
        for piece in next_rock.Pieces:
            piece_x = piece[0]
            piece_y = piece[1]
            height_list[piece_x] = max(height_list[piece_x], piece_y)
            pieces_set.add(piece)
        rock_count+=1

# 3067
part_1_result = max(Part1(input_line, 2022))
print("Part 1 Result: {}".format(part_1_result))