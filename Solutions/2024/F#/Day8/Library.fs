module Day8

open System.IO

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.map Seq.toArray
    |> Seq.toArray

let GetAntinodes (grid: char array array) =
    let inBounds (y, x) = 
        if y < 0 || x < 0 then 
            false
        else if y >= grid.Length || x >= grid[0].Length then 
            false
        else
            true
    let getFrequencyMap (freqMap: Map<char, (int*int) Set>) nextFreq =
        let (nextChar, nextPoint) = nextFreq
        match freqMap.TryFind nextChar with 
        | None -> freqMap.Add (nextChar, Set.singleton nextPoint)
        | Some(charSet) -> freqMap.Add (nextChar, Set.add nextPoint charSet)
    
    let antinodeFold acc _ pointSet = 
        let processPoint point = 
            let getAntinodes oppositePoint = 
                let (pointY, pointX) = point
                let (oppositeY, oppositeX) = oppositePoint
                let deltaY = oppositeY - pointY
                let deltaX = oppositeX - pointX

                let antiNode1 = (pointY - deltaY, pointX - deltaX)
                let antiNode2 = (oppositeY + deltaY, oppositeX + deltaX)

                let node1Set = 
                    if not (inBounds antiNode1) then 
                        Set.empty
                    else
                        Set.singleton antiNode1

                let node2Set = 
                    if not (inBounds antiNode2) then 
                        Set.empty
                    else
                        Set.singleton antiNode2

                Set.union node1Set node2Set
            pointSet
            |> Set.remove point
            |> Set.map getAntinodes
            |> Set.unionMany
        pointSet
        |> Set.map processPoint
        |> Set.unionMany
        |> Set.union acc


    
    let freqMap = seq {
        for y in 0..grid.Length - 1 do 
            for x in 0..grid[0].Length - 1 do 
                if grid[y][x] <> '.' then 
                    yield (grid[y][x], (y,x))
    }

    freqMap
    |> Seq.fold getFrequencyMap Map.empty 
    |> Map.fold antinodeFold Set.empty


let Part1 input = 
    GetAntinodes input
    |> Set.count

let Part2 input = 
    0