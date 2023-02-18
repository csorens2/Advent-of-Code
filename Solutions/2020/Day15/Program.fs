open System.IO

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.map (fun x -> x.Split[|','|])
    |> Seq.head
    |> Seq.map (fun x -> int x)

let NumberGame (nums: seq<int>) turns = 
    let rec rec_numberGame (numMap:Map<int,int>) lastNum currTurn endTurn = 
        if currTurn = endTurn then
            lastNum
        else
            let nextSpoken = 
                if not (numMap.ContainsKey(lastNum)) then
                    0
                else
                    currTurn - 1 - numMap[lastNum]
            let nextMap = 
                Map.add lastNum (currTurn - 1) numMap
            rec_numberGame nextMap  nextSpoken (currTurn + 1) endTurn
    let baseMap = 
        nums
        |> Seq.indexed
        |> Seq.take (Seq.length nums - 1)
        |> Seq.fold (fun (acc: Map<int,int>) (i,next) -> acc.Add(next, i + 1)) Map.empty
    let starterNumber = Seq.last nums
    let starterTurn = Seq.length nums + 1
    rec_numberGame baseMap starterNumber starterTurn (turns + 1)

let Part1 (input: seq<int>) = 
    NumberGame input 2020

let Part2 (input: seq<int>) = 
    NumberGame input 30000000

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 1025
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 129262
    0