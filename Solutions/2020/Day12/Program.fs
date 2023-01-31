open System.IO
open System.Text.RegularExpressions

type Action = 
    | Forward = 0
    | Rotate_Right = 1
    | Rotate_Left = 2
    | North = 3
    | East = 4
    | South = 5
    | West = 6 

type Boat(currX, currY, facing) = 
    member this.X = currX
    member this.Y = currY
    member this.Facing = facing

let ParseInput filepath = 
    let rx = Regex(@"([a-zA-Z])(\d+)", RegexOptions.Compiled)
    File.ReadLines(filepath)
    |> Seq.map (fun x ->
        let rxMatch = rx.Match(x)
        let actionMatch = 
            match rxMatch.Groups[1].Value with
            | "F" -> Action.Forward
            | "R" -> Action.Rotate_Right
            | "L" -> Action.Rotate_Left
            | "N" -> Action.North
            | "S" -> Action.South
            | "E" -> Action.East
            | "W" -> Action.West
            | _ -> failwith "Unexpected Action"
        (actionMatch, int(rxMatch.Groups[2].Value)))

let part1 (input: seq<Action * int>) = 
    let directionActionDict = [
        (Action.North, (fun (x, y) -> (x,y+1))),
        (Action.South, (fun (x, y) -> (x,y-1))),
        (Action.East, (fun (x, y) -> (x+1,y))),
        (Action.West, (fun (x, y) -> (x-1,y)))
    ]
    let processAction (boatX,boatY) facing (nextAction,actionVal) = 
        if List.contains nextAction (Map.keys directionActionDict) then
            directionActionDict(nextAction) (boat)
        else
            

    input
    |> Seq.fold (fun acc (nextAct, nextVal) -> ) Boat(0, 0, 90)

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    //let part1Result = getOccupiedCount input getNearbyCount 4
    //printfn "Part 1 Result: %d" part1Result 
    //let part2Result = getOccupiedCount input getVisibleCount 5
    //printfn "Part 2 Result: %d" part2Result
    0