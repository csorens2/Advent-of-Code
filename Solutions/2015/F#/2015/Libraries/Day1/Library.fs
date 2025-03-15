module Day1

open System.IO

type Instruction = 
    | Up
    | Down

let ParseInput filepath = 
    let mapFunc toMap = 
        match toMap with 
        | '(' -> Up
        | ')' -> Down
        | _ -> failwith "Unknown char"

    File.ReadLines(filepath)
    |> Seq.head
    |> Seq.toList
    |> List.map mapFunc

let Part1 input = 
    let mapFunc toMap = 
        match toMap with 
        | Up -> 1
        | Down -> -1

    input
    |> List.map mapFunc
    |> List.sum

let Part2 input = 
    let rec traverseFloors (remainingInstructions: Instruction list) (floor: int) (pos: int) = 
        if remainingInstructions.IsEmpty then 
            failwith "Never reached basement"
        else 
            let nextInstruction = remainingInstructions.Head
            let nextFloor = 
                match nextInstruction with 
                | Up -> floor + 1
                | Down -> floor - 1
            if nextFloor < 0 then 
                pos
            else 
                traverseFloors remainingInstructions.Tail nextFloor (pos + 1)

    traverseFloors input 0 1