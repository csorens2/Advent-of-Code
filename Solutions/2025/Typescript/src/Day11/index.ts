import { HashMap, List, Match, Option  } from "effect";
type HashMap<K, V> = HashMap.HashMap<K, V>
type List<V> = List.List<V>
import { readFileSync } from 'fs';

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

function GetNumPaths<T>(graphMap: Map<T, List<T>>, baseCaseMemoizationMap: Map<T, number>, startNode: T): number {

    const test = (): number => {
        return 0
    }

    const buildNumPathsMap = (memoizationMap: HashMap<T, number>, currNode: T): HashMap<T, number> => {

        if (HashMap.has(currNode)(memoizationMap)) {
            return memoizationMap
        }
        else {

            const foldSubGraphs = (accMap: HashMap<T, number>, nextNode: T): HashMap<T, number> => {
                const subGraph = buildNumPathsMap(accMap, nextNode)
                const toAdd = HashMap.unsafeGet(nextNode)(subGraph)

                return

            }

            return HashMap.empty()
        }


    }

    return 0
}

export function Part1(input: HashMap<string, List<string>>): number {
    return 0
}





