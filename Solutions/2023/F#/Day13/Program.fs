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
    let rec compareStacks firstStack secondStack = 
        match List.isEmpty firstStack with 
        | true -> true
        | false -> 
            let topCurr = List.head firstStack
            let topRemain = List.head secondStack
            if topCurr = topRemain then 
                compareStacks (List.tail firstStack) (List.tail secondStack)
            else
                false
    let rec isMirror (topStack: Material array list) (bottomStack: Material array list) = 
        if List.isEmpty bottomStack then
            false
        else
            if compareStacks topStack bottomStack then 
                true
            else
                let nextTopStack = (List.head bottomStack) :: topStack
                let nextBottomStack = List.tail bottomStack
                isMirror nextTopStack bottomStack

    let verticalArrays = 
        input 
        |> List.map (fun materials ->
            let vertical )
    
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