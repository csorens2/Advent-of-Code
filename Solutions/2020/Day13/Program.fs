open System.IO

let ParseInput filepath = 
    let lines = 
        File.ReadLines(filepath) 
        |> Seq.toList
    let departureTime = int lines.Head
    let busTimes = 
        lines.Tail.Head.Split ','
        |> seq
    (departureTime, busTimes)

let Part1 ((departTime, input) : int * seq<string>) = 
    let busIDs = 
        input
        |> Seq.filter (fun x -> x <> "x")
        |> Seq.map (int)
    let infiniteBus busID = 
        busID 
        |> Seq.unfold (fun state -> Some(state, state + busID))
    let (firstBusID, firstTime) = 
        busIDs
        |> Seq.map (fun x -> infiniteBus x)
        |> Seq.map (fun x -> x |> Seq.find (fun y -> y >= departTime))
        |> Seq.zip busIDs
        |> Seq.sortBy (fun (_,firstTime) -> firstTime)
        |> Seq.head
    (firstTime - departTime) * firstBusID

[<EntryPoint>]
let main _ =
    let (departTime, input) = ParseInput("Input.txt")
    let part1Result = Part1 (departTime, input)
    printfn "Part 1 Result: %d" part1Result // 119
    // let part2Result = Part2 input
    // printfn "Part 2 Result: %d" part2Result // 
    0