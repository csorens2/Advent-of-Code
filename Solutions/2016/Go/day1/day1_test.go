package day1

import "testing"

func TestPart1(t *testing.T) {
	instructions := parseFile("input.txt")

	actual := Part1(instructions)
	expected := 291
	if actual != expected {
		t.Errorf("Expected:%d Actual:%d", expected, actual)
	}

}

func TestPart2(t *testing.T) {
	instructions := parseFile("input.txt")

	actual := Part2(instructions)
	expected := 159
	if actual != expected {
		t.Errorf("Expected:%d Actual:%d", expected, actual)
	}
}
