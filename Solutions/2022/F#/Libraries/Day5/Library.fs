module Day5

open System.IO
open System
open System.Text.RegularExpressions

type MoveCommand = {
    Num: int
    From: int
    To: int
}

let ParseInput filepath = 
    
    let lines = 
        File.ReadLines(filepath)
        |> Seq.toList

    let crateLines = 
        lines
        |> List.filter (fun line -> line.Contains("]"))
        |> List.rev
        |> List.map (fun line -> Seq.toArray line)

    let numPosition = 
        lines
        |> List.filter (fun line -> line.Contains("1") && (not (line.Contains("make"))))
        |> List.head
        |> Seq.toList
        |> List.indexed
        |> List.filter (fun (_,posChar) -> posChar <> ' ')
        |> List.map (fun (x, posChar) -> (x, Convert.ToInt32(posChar.ToString())))

    let getCrateRow (arrPos, num) = 
        let rec processRow (remainingRows: char array list) = seq {
            if not (List.isEmpty remainingRows) then 
                let nextCrateRow = remainingRows.Head
                if not (nextCrateRow.[arrPos] = ' ') then 
                    yield nextCrateRow.[arrPos]
                    yield! processRow remainingRows.Tail
        }
        let numCrates = 
            (processRow crateLines)
            |> Seq.toList 
            |> List.rev
        (num, numCrates)

    let crateMap = 
        numPosition
        |> List.map getCrateRow
        |> List.fold (fun acc (nextNum, nextStack) -> Map.add nextNum nextStack acc) Map.empty

    let getMoveCommand line = 
        let regexMatch = Regex.Match(line, "move (\d+) from (\d+) to (\d+)")
        {
            MoveCommand.Num = Int32.Parse regexMatch.Groups[1].Value;
            From = Int32.Parse regexMatch.Groups.[2].Value;
            To = Int32.Parse regexMatch.Groups.[3].Value
        }

    let moveLines = 
        lines
        |> List.filter (fun line -> line.Contains("move"))
        |> List.map getMoveCommand

    (crateMap, moveLines)

let ProcessMoves sameOrder crateMap nextMove = 
    let takeFromStack = 
        crateMap
        |> Map.find nextMove.From
        |> List.take nextMove.Num
    let toMove = 
        if sameOrder then 
            takeFromStack
        else 
            List.rev takeFromStack

    let nextFrom = 
        crateMap
        |> Map.find nextMove.From
        |> List.skip nextMove.Num
    let nextTo = 
        crateMap 
        |> Map.find nextMove.To
        |> List.append toMove
    crateMap
    |> Map.add nextMove.From nextFrom
    |> Map.add nextMove.To nextTo

let Part1 input = 
    let (initCrateMap, initMoveList) = input

    initMoveList
    |> List.fold (ProcessMoves false) initCrateMap
    |> Map.toList
    |> List.sortBy (fun (stackNum, _) -> stackNum)
    |> List.fold (fun accString (_, stackList) -> accString + (string (List.head stackList))) ""

let Part2 input = 
    let (initCrateMap, initMoveList) = input

    initMoveList
    |> List.fold (ProcessMoves true) initCrateMap
    |> Map.toList
    |> List.sortBy (fun (stackNum, _) -> stackNum)
    |> List.fold (fun accString (_, stackList) -> accString + (string (List.head stackList))) ""