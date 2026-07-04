import { ParseInput, Part1, Part2 } from "../../src/Day11/index.js";
import { describe, test, expect } from 'vitest';
describe('Day 11 Tests', () => {
    test('Part 1 Example', () => {
        const input = ParseInput("tests/Day11/Example1.txt");
        const result = Part1(input);
        expect(result).toBe(5);
    });
    test('Part 1', () => {
        const input = ParseInput("tests/Day11/Input.txt");
        const result = Part1(input);
        expect(result).toBe(523);
    });
    test('Part 2 Example', () => {
        const input = ParseInput("tests/Day11/Example2.txt");
        const result = Part2(input);
        expect(result).toBe(2);
    });
    test('Part 2', () => {
        const input = ParseInput("tests/Day11/Input.txt");
        const result = Part2(input);
        expect(result).toBe(517315308154944);
    });
});
//# sourceMappingURL=index.test.js.map