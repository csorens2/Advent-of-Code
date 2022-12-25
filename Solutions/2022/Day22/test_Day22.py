import Day22
from Day22 import Orientation
from Day22 import Space

def test_ParseGrid_FillsInSpace():
    test_grid = [
        ".",
        "...",
        "."
    ]
    expected_grid = [
        [Space.Floor, Space.Empty, Space.Empty],
        [Space.Floor, Space.Floor, Space.Floor],
        [Space.Floor, Space.Empty, Space.Empty],
    ]
    actual = Day22.ParseGrid(test_grid)
    for (y, row) in enumerate(expected_grid):
        for (x, _) in enumerate(row):
            assert expected_grid[y][x] == actual[y][x]

def test_GetNextPos_CardinalDirections():
    test_grid = Day22.ParseGrid([
        "...",
        "...",
        "..."
    ])
    test_point = (1,1)
    orientation_tuples = [
        (Orientation.Up, (0,1)),
        (Orientation.Right, (1,2)),
        (Orientation.Down, (2,1)),
        (Orientation.Left, (1,0))
    ]
    for (orient, expected) in orientation_tuples:
        assert expected == Day22.GetNextPos(test_grid, test_point, orient)

def test_GetNextPos_WrapAround():
    test_grid = Day22.ParseGrid([
        "...",
        "...",
        "..."
    ])
    test_cases = [
        (Orientation.Up, (0,1), (2,1)),
        (Orientation.Down, (2,1), (0,1)),
        (Orientation.Left, (1,0), (1,2)),
        (Orientation.Right, (1,2), (1,0))
    ]
    for (orient, start, expected) in test_cases:
        assert expected == Day22.GetNextPos(test_grid, start, orient)

def test_GetNextPos_SkipsOpen():
    test_grid = Day22.ParseGrid([
        "...",
        ". .",
        "..."
    ])
    test_cases = [
        (Orientation.Up, (2,1), (0,1)),
        (Orientation.Down, (0,1), (2,1)),
        (Orientation.Left, (1,2), (1,0)),
        (Orientation.Right, (1,0), (1,2))
    ]
    for (orient, start, expected) in test_cases:
        assert expected == Day22.GetNextPos(test_grid, start, orient)

def test_GetNextPos_SkipsOpenAndWraps():
    test_grid = Day22.ParseGrid([
        ".. ",
    ])
    test_cases = [
        (Orientation.Right, (0,1), (0,0))
    ]
    for (orient, start, expected) in test_cases:
        assert expected == Day22.GetNextPos(test_grid, start, orient)

def test_RotateProperlyRotates():
    expected_left = [
        Orientation.Left,
        Orientation.Down,
        Orientation.Right,
        Orientation.Up,
        Orientation.Left
    ]
    expected_right = [
        Orientation.Right,
        Orientation.Down,
        Orientation.Left,
        Orientation.Up,
        Orientation.Right
    ]
    expected_tuples = [
        (Day22.Rotation.Left, expected_left),
        (Day22.Rotation.Right, expected_right)
    ]
    for (rotation, expecteds) in expected_tuples:
        test_person = Day22.Person(0,0, Orientation.Up)
        for expected in expecteds:
            test_person.Rotate(rotation)
            assert expected == test_person.Orientation