package day1

import (
	"log"
	"math"
	"os"
	"regexp"
	"strconv"
)

type Direction int

const (
	North Direction = iota
	East
	South
	West
)

type Instruction struct {
	Rotation string
	Distance int
}

type PosMapKey struct {
	xPos int
	yPos int
}

func parseFile(filepath string) []Instruction {
	content, err := os.ReadFile(filepath)
	if err != nil {
		log.Fatal(err)
	}

	fileContent := string(content)

	regexPattern := `([RL])(\d+)`
	re := regexp.MustCompile(regexPattern)

	var instructions []Instruction
	matchesWithGroups := re.FindAllStringSubmatch(fileContent, -1)
	for _, match := range matchesWithGroups {
		rotation := match[1]
		distance, _ := strconv.Atoi(match[2])

		nextInstruction := Instruction{
			Rotation: rotation,
			Distance: distance,
		}
		instructions = append(instructions, nextInstruction)
	}
	return instructions
}

func RotateRight(direction Direction) Direction {
	switch direction {
	case North:
		return East
	case East:
		return South
	case South:
		return West
	default:
		return North
	}
}

func RotateLeft(direction Direction) Direction {
	switch direction {
	case North:
		return West
	case West:
		return South
	case South:
		return East
	default:
		return North
	}
}

func Part1(input []Instruction) int {
	facingDirection := North
	xPos := 0
	yPos := 0
	for _, instruction := range input {
		if instruction.Rotation == "R" {
			facingDirection = RotateRight(facingDirection)
		} else {
			facingDirection = RotateLeft(facingDirection)
		}

		distance := instruction.Distance
		switch facingDirection {
		case North:
			yPos += distance
		case East:
			xPos += distance
		case South:
			yPos -= distance
		default:
			xPos -= distance
		}
	}
	return int(math.Abs(float64(xPos)) + math.Abs(float64(yPos)))
}

func Part2(input []Instruction) int {
	facingDirection := North
	xPos := 0
	yPos := 0
	visited := make(map[PosMapKey]int)

	for _, instruction := range input {
		if instruction.Rotation == "R" {
			facingDirection = RotateRight(facingDirection)
		} else {
			facingDirection = RotateLeft(facingDirection)
		}

		for i := 0; i < instruction.Distance; i++ {
			switch facingDirection {
			case North:
				yPos += 1
			case East:
				xPos += 1
			case South:
				yPos -= 1
			default:
				xPos -= 1
			}

			nextPos := PosMapKey{
				xPos: xPos,
				yPos: yPos,
			}

			_, keyExists := visited[nextPos]
			if keyExists {
				return int(math.Abs(float64(xPos)) + math.Abs(float64(yPos)))
			} else {
				visited[nextPos] = 1
			}
		}
	}
	return -1
}
