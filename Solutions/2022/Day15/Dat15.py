import sys
import re
from enum import Enum

class GridSpace(Enum):
    Empty = '#'
    Beacon = 'B'
    Sensor = 'S'

def ParseInput(lines):
    for line in lines:
        values_regex = "(?:[=])([-]?\d+)"
        matches = list(map(lambda x: int(x), re.findall(values_regex, line)))
        sensor = (matches[0], matches[1])
        beacon = (matches[2], matches[3])
        yield (sensor, beacon)

file_path = sys.argv[1]
file = open(file_path, 'r')
input_lines = file.read().splitlines()
parsed_input = ParseInput(input_lines)

# Part 1 > 4886370
def GetRowDict(sensor_beacon_pairs, target_row):
    row_positions = {}
    can_reach_target_row = lambda sensor, travel_distance: True if abs(sensor[1] - target_row) <= travel_distance else False
    for sensor_beacon in sensor_beacon_pairs:
        next_sensor = sensor_beacon[0]
        next_beacon = sensor_beacon[1]
        row_positions.update({next_sensor: GridSpace.Sensor})
        row_positions.update({next_beacon: GridSpace.Beacon})
        total_travel = abs(next_sensor[0] - next_beacon[0]) + abs(next_sensor[1] - next_beacon[1])
        if not can_reach_target_row(next_sensor, total_travel):
            continue
        total_travel_shifted = total_travel - abs(target_row - next_sensor[1])
        starter_point = (next_sensor[0], target_row)
        lambda_list = [
            lambda point: (point[0] - 1, point[1]),
            lambda point: (point[0] + 1, point[1])
        ]
        if starter_point not in row_positions:
            row_positions[starter_point] = GridSpace.Empty
        for traversal_lambda in lambda_list:
            traversal_point = traversal_lambda(starter_point)
            for _ in range(0, total_travel_shifted):
                if traversal_point not in row_positions:
                    row_positions[traversal_point] = GridSpace.Empty
                traversal_point = traversal_lambda(traversal_point)
    return row_positions

#part_1_result = GetRowDict(parsed_input, 2000000)
#part_1_count = len(list(filter(lambda kvp: kvp[1] != GridSpace.Beacon and kvp[1] != GridSpace.Sensor, part_1_result.items())))
#print("Part 1 Result: {}".format(part_1_count))



# Part 2 > 11374534948438 (This will take a while to run)
def GetManhattanDistance(start_pos, end_pos):
    return abs(start_pos[0] - end_pos[0]) + abs(start_pos[1] - end_pos[1])

class Sensor:
    SensorPosition = None
    NearestBeacon = None
    ManhattanDistance = None
    def __init__(self, position, nearestBeacon):
        self.SensorPosition = position
        self.NearestBeacon = nearestBeacon
        self.ManhattanDistance = GetManhattanDistance(position, nearestBeacon)
    def PointInRange(self, point):
        manhat_distance = GetManhattanDistance(self.SensorPosition, point)
        return self.ManhattanDistance >= manhat_distance
    def GetSurroundingPoints(self):
        def PointOnEdge(point):
            manhat_distance = GetManhattanDistance(self.SensorPosition, point)
            return self.ManhattanDistance + 1 == manhat_distance
        traversal_lambdas = [
            lambda point: (point[0] + 1, point[1] - 1), 
            lambda point: (point[0] + 1, point[1] + 1), 
            lambda point: (point[0] - 1, point[1] + 1), 
            lambda point: (point[0] - 1, point[1] - 1), 
        ]
        starting_pos = (self.SensorPosition[0] - self.ManhattanDistance - 1, self.SensorPosition[1])
        current_pos = starting_pos
        for traversal_func in traversal_lambdas:
            yield current_pos
            while(PointOnEdge(traversal_func(current_pos))):
                current_pos = traversal_func(current_pos)
                yield current_pos

def Part2(sensor_beacon_pairs, max_dimension):
    sensor_list = []
    point_in_range = lambda point: point[0] >= 0 and point[1] >= 0 and point[0] < max_dimension and point[1] < max_dimension
    for sensor_beacon in sensor_beacon_pairs:
        sensor_list.append(Sensor(sensor_beacon[0], sensor_beacon[1]))
    for sensor in sensor_list:
        # Check the surrounding points on a given sensor..
        surrounding_points = sensor.GetSurroundingPoints()
        for point in surrounding_points:
            if not point_in_range(point):
                continue
            # ... against all the sensors
            covered = False
            for sensor_2 in sensor_list:
                if sensor_2.PointInRange(point):
                    covered = True
                    break
            if not covered:
                return point[0] * 4000000 + point[1]

#part_2_result = Part2(parsed_input, 4000000)
#print("Part 2 Result: {}".format(part_2_result))