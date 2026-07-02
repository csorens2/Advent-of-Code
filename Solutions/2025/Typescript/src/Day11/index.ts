import { type HashMap, HashMap as HM } from "effect";
import { readFileSync } from 'fs';

export function ParseInput(filename: string): HM<string, Array<string>> {

    const content = readFileSync(filename, 'utf-8');
    const lines = content.split(/\r?\n/);

    const parseLine = (line: string): [string, Array<string>] => {
        const regexString = String.raw`(\w+): (.+)`
        const match = line.match(regexString)

        const name = match![1]!
        const outputs = match![2]!.split(' ')
        return [name, outputs]
    }

    const test = HashMap.empty<String, Array<string>>()

    return new Map(lines.map(parseLine))
}

function GetNumPaths<T>(graphMap: Map<T, Array<T>>, baseCaseMemoizationMap: Map<T, number>, startNode: T): number {

    const buildNumPathsMap

    return 0
}

export function Part1(input: Map<string, Array<string>>): number {
    return 0
}





