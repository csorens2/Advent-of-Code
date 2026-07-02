import {ParseInput, Part1} from "../../src/Day11/index.ts";
import { describe, test, expect } from 'vitest'

describe('Day 11 Tests', () => {
    test('Part 1 Example', () => {
        const input = ParseInput("tests/Day11/Example1.txt")
        const result = Part1(input)
        expect(result).toBe(5)
    })
})