import sys
import functools

def MoveUp(pos):
    return (pos[0]-1, pos[1])
def MoveRight(pos):
    return (pos[0], pos[1]+1)
def MoveDown(pos):
    return (pos[0]+1, pos[1])
def MoveLeft(pos):
    return (pos[0], pos[1]-1)
move_list = [MoveUp, MoveRight, MoveDown, MoveLeft]

filePath = sys.argv[1]
file = open(filePath, 'r')
input_list = file.read().splitlines()

# [Y][X] From the upper left corner
tree_grid = list(map(lambda x: list(x), input_list))
    
# Part 1 > 1843
def CanTraverseToEdge(pos: tuple[int,int], init_height, grid, traversal_func):
    (y_pos, x_pos) = pos
    if y_pos < 0 or y_pos >= len(grid) or x_pos < 0 or x_pos >= len(grid[0]):
        return True
    curr_height = grid[y_pos][x_pos]
    if curr_height >= init_height:
        return False
    next_pos = traversal_func(pos)
    return CanTraverseToEdge(next_pos, init_height, grid, traversal_func)

def TreeVisibleFromEdge(pos: tuple[int,int], grid):
    for move_func in move_list:
        next_pos = move_func(pos)
        init_height = grid[pos[0]][pos[1]]
        result = CanTraverseToEdge(next_pos, init_height, grid, move_func)
        if result == True:
            return True
    return False

def CountTrees(grid):
    tree_count = 0
    for y in range(len(grid)):
        for x in range(len(grid[y])):
            if TreeVisibleFromEdge((y,x), grid):
                tree_count+=1
    return tree_count

part_1_solution = CountTrees(tree_grid)
print("Part 1 Solution: {}", part_1_solution)

# Part 2 > 180000

def CountTreesInDirection(pos: tuple[int,int], init_height, grid, traversal_func):
    (y_pos, x_pos) = pos
    if y_pos < 0 or y_pos >= len(grid) or x_pos < 0 or x_pos >= len(grid[0]):
        return 0
    curr_height = grid[y_pos][x_pos]
    if curr_height >= init_height:
        return 1
    next_pos = traversal_func(pos)
    return 1 + CountTreesInDirection(next_pos, init_height, grid, traversal_func)

def GetScenicScore(pos, grid):
    scenic_view_list = []
    for move_func in move_list:
        init_pos = move_func(pos)
        init_height = grid[pos[0]][pos[1]]
        result = CountTreesInDirection(init_pos, init_height, grid, move_func)
        scenic_view_list.append(result)
    score = functools.reduce(lambda acc, x: acc * x, scenic_view_list)
    return score

def GetHighestScenicScore(grid):
    best_score = -1
    for y in range(len(grid)):
        for x in range(len(grid[y])):
            score = GetScenicScore((y,x), grid)
            best_score = max(best_score, score)
    return best_score
    
part_2_solution = GetHighestScenicScore(tree_grid)
print("Part 2 Solution: {}", part_2_solution)           

test = 1
