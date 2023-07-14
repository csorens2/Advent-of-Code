open System
open System.IO


type Cube = {
    XPos: int
    YPos: int
    ZPos: int
}

let cubeToTuple cube = 
    (cube.XPos, cube.YPos, cube.ZPos)

type CubeBounds = {
    MinX: int
    MaxX: int
    MinY: int
    MaxY: int
    MinZ: int
    MaxZ: int
}

let ParseInput filepath = 
    let activeSpace = '#'
    File.ReadLines(filepath)
    |> Seq.indexed
    |> Seq.map (fun (yPos, line) -> 
        line
        |> Seq.indexed
        |> Seq.where (fun (_, cubeValue) -> cubeValue = activeSpace)
        |> Seq.map (fun (xPos, _ ) -> {Cube.XPos = xPos; YPos = yPos; ZPos = 0;}))
    |> Seq.collect id
    |> Set.ofSeq

let Part1 (input: Set<Cube>) = 
    let rec getBounds (remainingCubes: Cube list) (currBounds: CubeBounds) = 
        match remainingCubes with
        | [] -> 
            {
                currBounds with
                    MinX = currBounds.MinX - 1
                    MaxX = currBounds.MaxX + 1
                    MinY = currBounds.MinY - 1
                    MaxY = currBounds.MaxY + 1
                    MinZ = currBounds.MinZ - 1
                    MaxZ = currBounds.MaxZ + 1
            }
        | nextCube :: _ -> 
            getBounds
                (List.tail remainingCubes)
                { 
                    currBounds with
                        MinX = min currBounds.MinX nextCube.XPos
                        MaxX = max currBounds.MaxX nextCube.XPos
                        MinY = min currBounds.MinY nextCube.YPos
                        MaxY = max currBounds.MaxY nextCube.YPos
                        MinZ = min currBounds.MinZ nextCube.ZPos
                        MaxZ = max currBounds.MaxZ nextCube.ZPos
                }
    let baseBounds = {
        CubeBounds.MinX = System.Int32.MaxValue
        MaxX = System.Int32.MinValue
        MinY = System.Int32.MaxValue
        MaxY = System.Int32.MinValue
        MinZ = System.Int32.MaxValue
        MaxZ = System.Int32.MinValue
    }

    let processCycle cubeSet = 
        let countSurrounding targetCube cubesToCheck =
            let surroundingCubes cube = seq {
                let (xPos,yPos,zPos) = cubeToTuple cube
                for x in xPos-1..xPos+1 do
                    for y in yPos-1..yPos+1 do
                        for z in zPos-1..zPos+1 do
                            if (xPos,yPos,zPos) <> (x,y,z) then
                                yield {Cube.XPos = x; YPos = y; ZPos = z}
            
            }
            (surroundingCubes targetCube)
            |> Seq.where (fun cube -> Set.contains cube cubesToCheck)
            |> Seq.length
        let bounds = 
            getBounds (Set.toList cubeSet) baseBounds
        seq {
            for x in bounds.MinX..bounds.MaxX do
                for y in bounds.MinY..bounds.MaxY do
                    for z in bounds.MinZ..bounds.MaxZ do
                        let nextCube = 
                            {Cube.XPos = x; YPos = y; ZPos = z}
                        let surroundingCount = 
                            countSurrounding nextCube cubeSet
                        if cubeSet.Contains nextCube && (surroundingCount = 2 || surroundingCount = 3) then // Active
                            yield nextCube
                        elif surroundingCount = 3 then 
                            yield nextCube
        }
        |> Set.ofSeq
    let rec simulateCycles cubeSet remainingCycles = 
        match remainingCycles with 
        | 0 -> cubeSet
        | _ -> 
            simulateCycles (processCycle cubeSet) (remainingCycles - 1)
    simulateCycles input 6
    |> Set.count
            

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 359
    //let part2Result = Part2 input
    //printfn "Part 2 Result: %d" part2Result // 
    0