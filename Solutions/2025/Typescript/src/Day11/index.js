import { HashMap, List } from "effect";
import { readFileSync } from 'fs';
export function ParseInput(filename) {
    const content = readFileSync(filename, 'utf-8');
    const lines = content.split(/\r?\n/);
    const parseLine = (line) => {
        const regexString = String.raw `(\w+): (.+)`;
        const match = line.match(regexString);
        const name = match[1];
        const outputs = List.fromIterable(match[2].split(' '));
        return [name, outputs];
    };
    return HashMap.fromIterable(lines.map(parseLine));
}
function GetNumPaths(graphMap, baseCaseMemoizationMap, startNode) {
    const test = () => {
        return 0;
    };
    const buildNumPathsMap = (memoizationMap, currNode) => {
        if (HashMap.has(currNode)(memoizationMap)) {
            return memoizationMap;
        }
        else {
            return HashMap.empty();
        }
    };
    return 0;
}
export function Part1(input) {
    return 0;
}
//# sourceMappingURL=index.js.map