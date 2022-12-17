import sys
import re
from enum import Enum
import functools
import copy

class Valve:
    def __init__(self, name: str, flowRate: int, tunnels: list[str]):
        self.Name = name
        self.Flowrate = flowRate
        self.Tunnels = tunnels



def ParseInput(lines):
    valve_dict = {}
    for line in lines:
        regex = "((?:Valve (\w+) has flow rate=(\d+); tunnel(s)? lead(s)? to valve(s)?)|((\w+)))"
        matches = re.findall(regex, line)
        name = matches[0][1]
        flowrate = int(matches[0][2])
        tunnels = []
        match_iter = iter(matches)
        next(match_iter)
        for match in match_iter:
            tunnels.append(match[0])
        valve_dict[name] = Valve(name, flowrate, tunnels)
    return valve_dict

file_path = sys.argv[1]
file = open(file_path, 'r')
input_lines = file.read().splitlines()
parsed_input = ParseInput(input_lines)

# Part 1
def Part1(valve_dict: dict[str, Valve], starter_time: int):
    def Rec_ProcessValve(valve_name: str, current_flow: int, total_pressure: int, clock: int, opened_valves: set[str], visited_valves):
        if clock >= 30:
            return total_pressure
        visited_valves.append(valve_name)
        print(visited_valves)
        curr_valve = valve_dict[valve_name]
        opened_valve_result = 0
        if valve_name not in opened_valves and curr_valve.Flowrate != 0:
            open_clock = clock + 1
            open_current_flow = current_flow + curr_valve.Flowrate # Open the valve ...
            open_total_pressure = total_pressure + open_current_flow # releasing presuure
            open_opened_valves = copy.deepcopy(opened_valves)
            open_opened_valves.add(valve_name)
            # And then moving to...
            opened_valve_result = functools.reduce(lambda acc, next_valve: max(acc, Rec_ProcessValve(next_valve, open_current_flow, open_total_pressure, open_clock + 1, open_opened_valves, copy.deepcopy(visited_valves))), curr_valve.Tunnels, 0)
        # The branch where we didn't open the valve
        total_pressure +=  current_flow
        return max(opened_valve_result, functools.reduce(lambda acc, next_valve: max(acc, Rec_ProcessValve(next_valve, current_flow, total_pressure, clock + 1, opened_valves, copy.deepcopy(visited_valves))), curr_valve.Tunnels, 0))
        
    # result = Rec_ProcessValve("AA", 0, 0, 1, set())
    Rec_ProcessValve("AA", 0, 0, 1, set(), [])
    test = 1



test = Part1(parsed_input, 30)