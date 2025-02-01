import sys
import functools

class Directory:
    def __init__(self, name, parent_dir):
        self.Name = name
        self.Files = []
        self.ChildDirectories = []
        self.ParentDirectory = parent_dir

class File:
    def __init__(self, name, size):
        self.Name = name
        self.Size = size

def ParseInput(file_path):
    file = open(file_path, 'r')
    input_list = file.read().splitlines()
    input_iter = iter(input_list)

    new_operation_char = "$"
    above_directory_name = ".."
    change_directory_operation = "$ cd"
    list_directory_operation = "$ ls"

    root_directory = Directory("/", None)
    current_directory = root_directory
    next(input_iter)
    for line in input_iter:
        # Processing a new command
        if line.startswith(new_operation_char):
            if line.startswith(change_directory_operation):
                dir_name = line[len(change_directory_operation)+1:]
                if dir_name == above_directory_name:
                    current_directory = current_directory.ParentDirectory
                else:
                    current_directory = next(filter(lambda x: x.Name == dir_name, current_directory.ChildDirectories))
            if line.startswith(list_directory_operation):
                continue
        # Currently processing an 'ls' command file
        elif not line.startswith(new_operation_char):
            line_split = line.split(" ")
            first_part = line_split[0]
            second_part = line_split[1]
            # File
            if first_part.isdigit():
                file_size = int(first_part)
                new_file = File(second_part, file_size)
                current_directory.Files.append(new_file)
            else: # Directory
                new_dir = Directory(second_part, current_directory)
                current_directory.ChildDirectories.append(new_dir)
    return root_directory

def PrintDirectoryTree(curr_dir: Directory, depth: int):
    print("{}- {} (dir)".format(("  "*depth), curr_dir.Name))
    for child_dir in curr_dir.ChildDirectories:
        next_depth = depth + 1
        PrintDirectoryTree(child_dir, next_depth)
    for file in curr_dir.Files:
        print("{}- {} (file, size={})".format(("  "*(depth+1)), file.Name, file.Size))

file_path = sys.argv[1]
parsed_tree_root = ParseInput(file_path)
# PrintDirectoryTree(parsed_tree_root, 0)

# Part 1 > 1517599
def CalculateDirectoriesSum(curr_directory: Directory):
    directory_total = 0
    at_most_total = 0
    for directory in curr_directory.ChildDirectories:
        (child_directory_total, child_at_most_total) = CalculateDirectoriesSum(directory)
        directory_total += child_directory_total
        at_most_total += child_at_most_total

    file_total = functools.reduce(lambda acc, next: acc + next.Size, curr_directory.Files, 0)
    directory_total += file_total

    if directory_total < 100000:
        at_most_total += directory_total
    return (directory_total, at_most_total)

(_, part_1_result) = CalculateDirectoriesSum(parsed_tree_root)
print("Part 1 Result: {}".format(part_1_result))

# Part 2 > 2481982
# Return -> (Running Total, curr_list)
def GetDirectorySizeList(curr_directory:Directory, curr_list: list):
    file_total = functools.reduce(lambda acc, next: acc + next.Size, curr_directory.Files, 0)
    directory_total = functools.reduce(lambda acc, next: acc + GetDirectorySizeList(next, curr_list)[0], curr_directory.ChildDirectories, 0)
    running_total = file_total + directory_total
    curr_list.append(running_total)
    return (running_total, curr_list)

(total_disk_usage, directory_size_list) = GetDirectorySizeList(parsed_tree_root, [])
total_filesystem_space = 70000000
needed_unused_space = 30000000
total_used_space = total_disk_usage
current_unused_space = total_filesystem_space - total_used_space
needed_space = needed_unused_space - current_unused_space

directory_size_list.sort()
to_delete = next(filter(lambda x: x > needed_space, directory_size_list))
print("Part 2 Result: {}".format(to_delete))