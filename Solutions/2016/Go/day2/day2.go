package day2

import (
	"bufio"
	"os"
)

type Direction int

const (
	Up Direction = iota
	Right
	Down
	Left
)

func parseFile(filepath string) [][]Direction {
	file, _ := os.Open(filepath)
	defer file.Close()

	// Read each line
	var lines []string
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		lines = append(lines, line)
	}

	var directionLines [][]Direction
	for _, line := range lines {
		var directionLine []Direction
		runes := []rune(line)
		for _, lineChar := range runes {
			if lineChar == 'U' {
				directionLine = append(directionLine, Up)
			} else if lineChar == 'D' {
				directionLine = append(directionLine, Down)
			} else if lineChar == 'R' {
				directionLine = append(directionLine, Right)
			} else {
				directionLine = append(directionLine, Left)
			}
		}
		directionLines = append(directionLines, directionLine)
	}

	return directionLines
}

func InBounds(grid [][]string, yPos int, xPos int) bool {
	if yPos < 0 || yPos >= len(grid) {
		return false
	}
	if xPos < 0 || xPos >= len(grid[0]) {
		return false
	}
	if grid[yPos][xPos] == " " {
		return false
	}
	return true
}

func Move(yPos int, xPos int, direction Direction, grid [][]string) (int, int) {
	nextY := yPos
	nextX := xPos
	if direction == Up {
		nextY -= 1
	}
	if direction == Down {
		nextY += 1
	}
	if direction == Right {
		nextX += 1
	}
	if direction == Left {
		nextX -= 1
	}

	if InBounds(grid, nextY, nextX) {
		return nextY, nextX
	} else {
		return yPos, xPos
	}
}

func Part1(input [][]Direction) string {
	grid := [][]string{
		{"1", "2", "3"},
		{"4", "5", "6"},
		{"7", "8", "9"},
	}

	xPos := 1
	yPos := 1
	code := ""
	for _, line := range input {
		for _, direction := range line {
			yPos, xPos = Move(yPos, xPos, direction, grid)
		}
		code += grid[yPos][xPos]
	}

	return code
}

func Part2(input [][]Direction) string {
	grid := [][]string{
		{" ", " ", "1", " ", " "},
		{" ", "2", "3", "4", " "},
		{"5", "6", "7", "8", "9"},
		{" ", "A", "B", "C", " "},
		{" ", " ", "D", " ", " "},
	}

	xPos := 0
	yPos := 3
	code := ""
	for _, line := range input {
		for _, direction := range line {
			yPos, xPos = Move(yPos, xPos, direction, grid)
		}
		code += grid[yPos][xPos]
	}
	return code
}
