module Day13

open System.IO

type Material = 
    | Ash
    | Rock

let ParseInput filepath = 
    let rec parsePattern (remainingLines: string list) = seq {
        if remainingLines <> List.empty then 
            let nextPattern = 
                remainingLines
                |> List.takeWhile (fun line -> line <> "")
                |> List.map (fun materialLine -> 
                    materialLine
                    |> Seq.map (fun mat -> 
                        match mat with
                        | '.' -> Ash
                        | '#' -> Rock
                        | _ -> failwith $"Unknown material type: {mat}")
                    |> Seq.toArray)
                |> List.toArray
            yield nextPattern
            let nextLines = 
                remainingLines
                |> Seq.skipWhile (fun line -> line <> "")
                |> Seq.skipWhile (fun line -> line = "")
                |> Seq.toList
            yield! parsePattern nextLines
    }
    
    File.ReadLines(filepath)
    |> Seq.toList
    |> parsePattern
    |> Seq.toList
        

let Part1 input = 
    0

let Part2 input = 
    0

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    //let part1Result = Part1 input
    //printfn "Part 1 Result: %d" part1Result // 
    //let part2Result = Part2 input
    //printfn "Part 2 Result: %d" part2Result // 
    0