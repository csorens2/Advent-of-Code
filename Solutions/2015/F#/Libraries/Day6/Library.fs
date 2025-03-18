module Day6

open System.IO
open System.Text.RegularExpressions

type Action = 
    | TurnOn
    | TurnOff
    | Toggle

type Instruction = {
    Action: Action
    From: int*int
    To: int*int
}

let ParseInput filepath = 
    let mapLines (line: string) = 
        let (regexAction, regexMatch) = 
            let regexOn = Regex("turn on (\d+),(\d+) through (\d+),(\d+)")
            let regexOff = Regex("turn off (\d+),(\d+) through (\d+),(\d+)")
            let regexToggle = Regex("toggle (\d+),(\d+) through (\d+),(\d+)")
            if regexOn.IsMatch(line) then 
                (TurnOn, regexOn.Match(line))
            else if regexOff.IsMatch(line) then 
                (TurnOff, regexOff.Match(line))
            else
                (Toggle, regexToggle.Match(line))
        {
            Instruction.Action = regexAction
            From = (int regexMatch.Groups[1].Value, int regexMatch.Groups[2].Value)
            To = (int regexMatch.Groups[3].Value, int regexMatch.Groups[4].Value)
        }     

    File.ReadLines(filepath)
    |> Seq.map mapLines
    |> Seq.toList

let Part1 input = 
    let instructionsFold accOuter nextInstruction =
        let gridActionFold accInner nextPoint = 
            if nextInstruction.Action = TurnOn then 
                Map.add nextPoint true accInner
            else if nextInstruction.Action = TurnOff then 
                Map.add nextPoint false accInner
            else 
                let prevVal = Map.find nextPoint accInner
                Map.add nextPoint (not prevVal) accInner
            
        let (fromY, fromX) = nextInstruction.From
        let (toY, toX) = nextInstruction.To
        let gridSeq = seq {
            for y in fromY..toY do 
                for x in fromX..toX do 
                    yield (y,x)
        }

        gridSeq
        |> Seq.fold gridActionFold accOuter


    let startSeq = seq {
        for y in 0..999 do 
            for x in 0..999 do 
                yield ((y,x), false)
    }
    let startMap = Map.ofSeq startSeq
    
    input
    |> List.fold instructionsFold startMap
    |> Map.toList
    |> List.filter (fun (_, pointVal) -> pointVal = true)
    |> List.length

let Part2 input = 
    let instructionsFold accOuter next =
        let gridActionFold accInner nextPoint = 
            let prevVal = Map.find nextPoint accInner
            if next.Action = TurnOn then 
                Map.add nextPoint (prevVal + 1) accInner
            else if next.Action = TurnOff && prevVal > 0 then 
                Map.add nextPoint (prevVal - 1) accInner
            else if next.Action = TurnOff && prevVal = 0 then 
                accInner
            else
                Map.add nextPoint (prevVal + 2) accInner
            
        let (fromY, fromX) = next.From
        let (toY, toX) = next.To
        let gridSeq = seq {
            for y in fromY..toY do 
                for x in fromX..toX do 
                    yield (y,x)
        }

        gridSeq
        |> Seq.fold gridActionFold accOuter


    let startSeq = seq {
        for y in 0..999 do 
            for x in 0..999 do 
                yield ((y,x), 0)
    }
    let startMap = Map.ofSeq startSeq
    
    input
    |> List.fold instructionsFold startMap
    |> Map.toList
    |> List.map (fun (_, pointVal) -> pointVal)
    |> List.sum