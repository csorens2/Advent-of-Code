open System.IO

type Space = Open = '.' | Tree = '#'

let ParseInput filepath = 
    File.ReadLines(filepath) 
    |> Seq.map(fun row -> 
        row 
        |> Seq.map (fun x -> LanguagePrimitives.EnumOfValue<char, Space>(x))
        |> Seq.toList
    )
    |> Seq.toList

let CountTrees (grid: Space list list) ((deltaY, deltaX):(int*int)) = 
    let rec TraverseGrid y x trees = 
        let nextY = y + deltaY
        if nextY >= grid.Length then
            trees
        else
            let nextX = if x + deltaX < grid[nextY].Length then x + deltaX else (x + deltaX) % grid[nextY].Length
            let nexttrees = if grid[nextY][nextX] = Space.Tree then trees + 1 else trees
            TraverseGrid nextY nextX nexttrees
    TraverseGrid 0 0 0

let Part2 grid = 
    [
        (1, 1);
        (1, 3);
        (1, 5);
        (1, 7);
        (2, 1)
    ]
    |> List.fold (fun acc next -> acc * uint32(CountTrees grid next)) 1u

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = CountTrees input (1, 3)
    printfn "Part 1 Result: %d" part1Result 
    let part2Result = Part2(input)
    printfn "Part 2 Result: %d" part2Result
    0