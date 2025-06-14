package day2

import "testing"

func TestExamplePart1(t *testing.T) {
	instructions := parseFile("example.txt")

	actual := Part1(instructions)
	expected := "1985"
	if actual != expected {
		t.Errorf("Expected:%s Actual:%s", expected, actual)
	}
}

func TestPart1(t *testing.T) {
	instructions := parseFile("input.txt")

	actual := Part1(instructions)
	expected := "56983"
	if actual != expected {
		t.Errorf("Expected:%s Actual:%s", expected, actual)
	}
}

func TestExamplePart2(t *testing.T) {
	instructions := parseFile("example.txt")

	actual := Part2(instructions)
	expected := "5DB3"
	if actual != expected {
		t.Errorf("Expected:%s Actual:%s", expected, actual)
	}
}

func TestPart2(t *testing.T) {
	instructions := parseFile("input.txt")

	actual := Part2(instructions)
	expected := "8B8B1"
	if actual != expected {
		t.Errorf("Expected:%s Actual:%s", expected, actual)
	}
}
