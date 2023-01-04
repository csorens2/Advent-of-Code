open System.IO
open System

let ParseInput filepath = 
    File.ReadLines(filepath)      
    |> Seq.map(fun x -> int(x))
    |> Seq.toList

let rec FindSum nums remainingSum remainingnums = 
    if remainingnums = 1 then
        if nums |> Seq.contains(remainingSum) then
            remainingSum
        else
            0
    else
        nums |> Seq.reduce(fun acc next ->
            Math.Max(
                acc, 
                next * FindSum (nums |> Seq.except([next])) (remainingSum - next) (remainingnums - 1)))

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let day1Result = FindSum input 2020 2
    printfn "Part 1 Result: %A" day1Result
    let day2Result = FindSum input 2020 3
    printfn "Part 2 Result: %A" day2Result
    0