open System.IO
open System.Text.RegularExpressions

type Direction = 
    | North = 0
    | East = 1
    | South = 2
    | West = 3

type Action = 
    | Forward of int
    | Move of Direction * int
    | Rotate of bool * int

type Part = 
    | One = 0
    | Two = 0

let ParseInput filepath = 
    let rx = Regex(@"([a-zA-Z])(\d+)", RegexOptions.Compiled)
    File.ReadLines(filepath)
    |> Seq.map (fun x ->
        let rxMatch = rx.Match(x)
        let rxAction = rxMatch.Groups[1].Value
        let rxValue = int(rxMatch.Groups[2].Value)
        match rxAction with
        | "F" -> Forward(rxValue)
        | "R" -> Rotate(true, rxValue / 90)
        | "L" -> Rotate(false, rxValue / 90)
        | "N" -> Move(Direction.North, rxValue)
        | "S" -> Move(Direction.South, rxValue)
        | "E" -> Move(Direction.East, rxValue)
        | "W" -> Move(Direction.West, rxValue)
        | _ -> failwith "Unexpected Action")

let processMove (sourceX,sourceY) (dirX,dirY) moveVal = 
            (sourceX + (dirX * moveVal), sourceY + (dirY * moveVal))

let processRotate (waypointXY: int*int) (direction: bool) (num:int) = 
    let rotateDict = 
        Map.empty.
            Add(true, fun (x,y) -> (y, -1 * x)).
            Add(false, fun (x,y) -> (-1*y,x))
    [0..num-1]
    |> Seq.fold (fun acc _ -> rotateDict[direction] acc) waypointXY

let dirToPoint = 
    Map.empty.
        Add(Direction.North, (0,1)).
        Add(Direction.East, (1,0)).
        Add(Direction.South, (0,-1)).
        Add(Direction.West, (-1,0));

let part1 (input : seq<Action>) = 
    let rec boatPos (boatXY: int*int) (waypointXY: int*int) (remainingActions: seq<Action>) = 
        if Seq.isEmpty remainingActions then
            let (boatX, boatY) = boatXY
            abs(boatX) + abs(boatY)
        else
            let nextActions = Seq.tail remainingActions
            match Seq.head remainingActions with
            | Forward value -> boatPos (processMove boatXY waypointXY value) waypointXY nextActions
            | Move (dir,value) -> boatPos (processMove boatXY dirToPoint[dir] value) waypointXY nextActions
            | Rotate (dir, value) -> boatPos boatXY (processRotate waypointXY dir value) nextActions
    boatPos (0,0) (1,0) input

let part2 (input: seq<Action>) = 
    let rec boatPos (boatXY: int*int) (waypointXY: int*int) (remainingActions: seq<Action>) = 
        if Seq.isEmpty remainingActions then
            let (boatX, boatY) = boatXY
            abs(boatX) + abs(boatY)
        else
            let nextActions = Seq.tail remainingActions
            match Seq.head remainingActions with
            | Forward value -> boatPos (processMove boatXY waypointXY value) waypointXY nextActions
            | Move (dir,value) -> boatPos boatXY (processMove waypointXY dirToPoint[dir] value) nextActions
            | Rotate (dir, value) -> boatPos boatXY (processRotate waypointXY dir value) nextActions
    boatPos (0,0) (10,1) input
            

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = part1 input
    printfn "Part 1 Result: %d" part1Result // 1687
    let part2Result = part2 input
    printfn "Part 2 Result: %d" part2Result // 20873
    0