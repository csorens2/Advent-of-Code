open System.IO
open System

let ParseInput filepath = 
    File.ReadLines(filepath)      
    |> Seq.map(fun x -> int(x))
    |> Seq.cache

let rec FindSum numList remainingSum remainingNums = 
    match remainingNums with
    | 1 -> 
        match Seq.contains remainingSum numList with 
        | true -> remainingSum
        | false -> 0
    | _ ->
        numList
        |> Seq.fold(fun acc nextNum -> 
            let nextRemainingSum = remainingSum - nextNum
            let nextNumList = 
                numList
                |> Seq.where (fun num -> num <= nextRemainingSum)
            match Seq.isEmpty nextNumList with
            | true -> acc
            | false ->
                Math.Max( // We use Math.max to easily deal with the initial case of 0
                    acc,
                    nextNum * FindSum nextNumList nextRemainingSum (remainingNums - 1))) 0

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let day1Result = FindSum input 2020 2 // 974304
    printfn "Part 1 Result: %A" day1Result
    let day2Result = FindSum input 2020 3 // 236430480
    printfn "Part 2 Result: %A" day2Result
    0