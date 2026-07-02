import { ParseInput, Test2 } from "../../src/Day11/index.js";
import { describe, it, expect } from 'vitest';
describe('math utilities', () => {
    it('adds two numbers correctly', () => {
        expect(2 + 3).toBe(5);
        expect(-1 + 1).toBe(0);
        console.log('Is ParseInput a function?', typeof ParseInput);
        console.log('What is ParseInput?', ParseInput);
        console.log('Module exports:', Object.keys(ParseInput));
        ParseInput("");
    });
});
//# sourceMappingURL=index.test.js.map