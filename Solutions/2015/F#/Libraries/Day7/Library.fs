module Day7

open System.IO

open System.Text.RegularExpressions

type Instruction = 
    | Assign of string*string
    | And of string*string*string
    | Or of string*string*string
    | LShift of string*string*string
    | RShift of string*string*string
    | Not of string*string


let ParseInput filepath = 
    let mapInstruction (line: string) = 
        let assignRegex = Regex("^(\S+) -> (\S+)$")
        let andRegex = Regex("^(\S+) AND (\S+) -> (\S+)$")
        let orRegex = Regex("^(\S+) OR (\S+) -> (\S+)$")
        let lshiftRegex = Regex("^(\S+) LSHIFT (\S+) -> (\S+)$")
        let rshiftRegex = Regex("^(\S+) RSHIFT (\S+) -> (\S+)$")
        let notRegex = Regex("^NOT (\S+) -> (\S+)$")

        if assignRegex.IsMatch(line) then 
            let lineMatch = assignRegex.Match(line)
            Assign(lineMatch.Groups[1].Value, lineMatch.Groups[2].Value)
        else if andRegex.IsMatch(line) then 
            let lineMatch = andRegex.Match(line)
            And(lineMatch.Groups[1].Value, lineMatch.Groups[2].Value, lineMatch.Groups[3].Value)
        else if orRegex.IsMatch(line) then 
            let lineMatch = orRegex.Match(line)
            Or(lineMatch.Groups[1].Value, lineMatch.Groups[2].Value, lineMatch.Groups[3].Value)
        else if lshiftRegex.IsMatch(line) then 
            let lineMatch = lshiftRegex.Match(line)
            LShift(lineMatch.Groups[1].Value, lineMatch.Groups[2].Value, lineMatch.Groups[3].Value)
        else if rshiftRegex.IsMatch(line) then 
            let lineMatch = rshiftRegex.Match(line)
            RShift(lineMatch.Groups[1].Value, lineMatch.Groups[2].Value, lineMatch.Groups[3].Value)
        else if notRegex.IsMatch(line) then 
            let lineMatch = notRegex.Match(line)
            Not(lineMatch.Groups[1].Value, lineMatch.Groups[2].Value)
        else
            failwith "Unknown Instruction"
            

    File.ReadLines(filepath)
    |> Seq.map mapInstruction
    |> Seq.toList

let Part1 input = 
    0

let Part2 input = 
    0