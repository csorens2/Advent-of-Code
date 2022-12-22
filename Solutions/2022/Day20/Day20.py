import sys

class NumberNode:
    def __init__(self, value):
        self.Value = value
        self.Switched = False
        self.Prev = None
        self.Next = None
    def RemoveNode(self):
        self.Prev.Next = self.Next
        self.Next.Prev = self.Prev
        self.Next = None
        self.Prev = None
    def InsertNode(self, new_prev, new_next):
        new_prev.Next = self
        self.Prev = new_prev
        new_next.Prev = self
        self.Next = new_next
    def MoveBack(self):
        new_prev = self.Prev.Prev
        new_next = self.Prev
        self.RemoveNode()
        self.InsertNode(new_prev, new_next)
    def MoveForward(self):
        new_prev = self.Next
        new_next = self.Next.Next
        self.RemoveNode()
        self.InsertNode(new_prev, new_next)
    def ShiftByValue(self, mod):
        shift_amount = self.Value % mod if self.Value >= 0 else (self.Value % mod) * -1
        step = None
        if shift_amount < 0:
            step = -1
        else:
            step = 1
        for _ in range(0, shift_amount, step):
            if step == -1:
                self.MoveBack()
            else:
                self.MoveForward()

def ParseInput(lines, decryption_key):
    root_node = None
    last_node = None
    curr_node = None
    for line in lines:
        curr_node = NumberNode(int(line) * decryption_key)
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

def MixNodes(root: NumberNode, num_nodes, target_indexes, num_mixes):
    def FindZero(root_node):
        curr_node = root_node
        while(curr_node.Value != 0):
            curr_node = curr_node.Next
        return curr_node
    zero_node = FindZero(root)
    DumpList(zero_node)
    for i in range(0, num_mixes):
        switched_count = 0
        curr_node = root
        if i >= 1:
            curr_node.Switched = False
            curr_node = curr_node.Next
            while curr_node != root:
                curr_node.Switched = False
                curr_node = curr_node.Next
        while switched_count < num_nodes:
            if curr_node.Switched:
                curr_node = curr_node.Next
                continue
            next_node = curr_node.Next
            curr_node.ShiftByValue(num_nodes - 1)
            curr_node.Switched = True
            switched_count += 1
            curr_node = next_node
        DumpList(zero_node)
    curr_node = zero_node
    coordinate_sum = 0
    for i in range(0, max(target_indexes)+1):
        if i in target_indexes:
            coordinate_sum += curr_node.Value
        curr_node = curr_node.Next
    # DumpList(zero_node)
    return coordinate_sum

def DumpList(zero_node: NumberNode):
    print("0", end='')
    curr_node = zero_node.Next
    while curr_node != zero_node:
        print(", {}".format(curr_node.Value), end='')
        curr_node = curr_node.Next
    print()

# Part 1 > 7228
#part_1_mix_count = 1
#part_1_decryption_key = 1
#part_1_root_node = ParseInput(input_lines, part_1_decryption_key)
#part_1_result = MixNodes(part_1_root_node, len(input_lines), [1000, 2000, 3000], part_1_mix_count)
#print("Part 1 Result: {}".format(part_1_result))

# Part 2 > 
#part_2_mix_count = 10
#part_2_decryption_key = 811589153
#part_2_root_node = ParseInput(input_lines, part_2_decryption_key)
#part_2_result = MixNodes(part_2_root_node, len(input_lines), [1000, 2000, 3000], part_2_mix_count)
#print("Part 2 Result: {}".format(part_2_result))