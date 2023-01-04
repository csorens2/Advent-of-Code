open System.IO

let ParseInput filepath = 
    File.ReadLines(filepath) 
    |> Seq.map(fun row -> 
        row 
        |> Seq.map (fun x -> LanguagePrimitives.EnumOfValue<char, Space>(x))
        |> Seq.toList
    )
    |> Seq.toList

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    //let part1Result = CountTrees input (1, 3)
    //printfn "Part 1 Result: %d" part1Result 
    //let part2Result = Part2(input)
    //printfn "Part 2 Result: %d" part2Result
    0