package day3

import (
	"bufio"
	"os"
	"regexp"
	"strconv"
)

type Direction int

func parseFile(filepath string) [][]int {
	file, _ := os.Open(filepath)
	defer file.Close()

	// Read each line
	var lines []string
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		lines = append(lines, line)
	}

	var tupleLines [][]int
	for _, line := range lines {
		var tupleLine []int

		regexPattern := `(\d+)`
		re := regexp.MustCompile(regexPattern)
		matches := re.FindAllStringSubmatch(line, -1)
		for _, match := range matches {
			num, _ := strconv.Atoi(match[1])
			tupleLine = append(tupleLine, num)
		}

		tupleLines = append(tupleLines, tupleLine)
	}

	return tupleLines
}

func CountValid(numsList [][]int) int {
	validCount := 0
	for _, nums := range numsList {
		isValid :=
			(nums[0]+nums[1] > nums[2]) &&
				(nums[0]+nums[2] > nums[1]) &&
				(nums[1]+nums[2] > nums[0])
		if isValid {
			validCount += 1
		}
	}

	return validCount
}

func Part1(input [][]int) string {
	return strconv.Itoa(CountValid(input))
}

func Part2(input [][]int) string {
	var transformedInput [][]int

	for i := 0; i < len(input); i += 3 {
		left := []int{input[i][0], input[i+1][0], input[i+2][0]}
		mid := []int{input[i][1], input[i+1][1], input[i+2][1]}
		right := []int{input[i][2], input[i+1][2], input[i+2][2]}
		transformedInput = append(transformedInput, left, mid, right)
	}

	return strconv.Itoa(CountValid(transformedInput))
}
