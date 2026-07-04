import {Array, HashMap, List, Option} from "effect";
import {readFileSync} from 'fs';

type HashMap<K, V> = HashMap.HashMap<K, V>
type List<V> = List.List<V>

export function ParseInput(filename: string): HashMap<string, List<string>> {

    const content = readFileSync(filename, 'utf-8');
    const lines = content.split(/\r?\n/);

    const parseLine = (line: string): [string, List<string>] => {
        const regexString = String.raw`(\w+): (.+)`
        const match = line.match(regexString)

        const name = match![1]!
        const outputs = List.fromIterable(match![2]!.split(' '))
        return [name, outputs]
    }

    return HashMap.fromIterable(lines.map(parseLine))
}

function GetNumPaths<T>(graphMap: HashMap<T, List<T>>, baseCaseMemoizationMap: HashMap<T, number>, startNode: T): number {

    const buildNumPathsMap = (memoizationMap: HashMap<T, number>, currNode: T): HashMap<T, number> => {

        if (HashMap.has(currNode)(memoizationMap)) {
            return memoizationMap
        }
        else {

            const foldSubGraphs = (accMap: HashMap<T, number>, nextNode: T): HashMap<T, number> => {
                const subGraph = buildNumPathsMap(accMap, nextNode)
                const toAdd = HashMap.unsafeGet(nextNode)(subGraph)

                const toMatch = HashMap.get(currNode)(subGraph)

                if (Option.isNone(toMatch)) {
                    return HashMap.set(subGraph, currNode, toAdd)
                }
                else {
                    return HashMap.set(subGraph, currNode, toAdd + toMatch.value)
                }
            }

            return Array.reduce(
                HashMap.unsafeGet(graphMap, currNode),
                memoizationMap,
                foldSubGraphs)
        }
    }

    return HashMap.unsafeGet(
        buildNumPathsMap(baseCaseMemoizationMap, startNode),
        startNode)
}

export function Part1(input: HashMap<string, List<string>>): number {
    return GetNumPaths(input, HashMap.set(HashMap.empty(), "out", 1), "you")
}

export function Part2(input: HashMap<string, List<string>>): number {

    const svr = "svr"
    const dac = "dac"
    const fft = "fft"
    const out = "out"

    const defaultBaseCaseMap = HashMap.set(HashMap.empty(), out, 0)

    const SVR_to_FFT = GetNumPaths(input, (HashMap.set(defaultBaseCaseMap, fft, 1)), svr)
    const FFT_to_DAC = GetNumPaths(input, (HashMap.set(defaultBaseCaseMap, dac, 1)), fft)
    const DAC_to_OUT = GetNumPaths(input, (HashMap.set(defaultBaseCaseMap, out, 1)), dac)
    const numA = SVR_to_FFT * FFT_to_DAC * DAC_to_OUT

    const SVR_to_DAC = GetNumPaths(input, (HashMap.set(defaultBaseCaseMap, dac, 1)), svr)
    const DAC_to_FFT = GetNumPaths(input, (HashMap.set(defaultBaseCaseMap, fft, 1)), dac)
    const FFT_to_OUT = GetNumPaths(input, (HashMap.set(defaultBaseCaseMap, out, 1)), fft)
    const numB = SVR_to_DAC * DAC_to_FFT * FFT_to_OUT

    return numA + numB
}

