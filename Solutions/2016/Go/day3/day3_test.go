package day3

import "testing"

func TestExamplePart1(t *testing.T) {
	instructions := parseFile("example1.txt")

	actual := Part1(instructions)
	expected := "0"
	if actual != expected {
		t.Errorf("Expected:%s Actual:%s", expected, actual)
	}
}

func TestPart1(t *testing.T) {
	instructions := parseFile("input.txt")

	actual := Part1(instructions)
	expected := "982"
	if actual != expected {
		t.Errorf("Expected:%s Actual:%s", expected, actual)
	}
}

func TestExamplePart2(t *testing.T) {
	instructions := parseFile("example2.txt")

	actual := Part2(instructions)
	expected := "6"
	if actual != expected {
		t.Errorf("Expected:%s Actual:%s", expected, actual)
	}
}

func TestPart2(t *testing.T) {
	instructions := parseFile("input.txt")

	actual := Part2(instructions)
	expected := "1826"
	if actual != expected {
		t.Errorf("Expected:%s Actual:%s", expected, actual)
	}
}
