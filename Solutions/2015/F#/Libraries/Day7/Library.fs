module Day7

open System.IO

open System.Text.RegularExpressions

type Instruction = 
    | Assign of string*string
    | And of string*string*string
    | Or of string*string*string
    | LShift of string*int*string
    | RShift of string*int*string
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
            LShift(lineMatch.Groups[1].Value, int32 lineMatch.Groups[2].Value, lineMatch.Groups[3].Value)
        else if rshiftRegex.IsMatch(line) then 
            let lineMatch = rshiftRegex.Match(line)
            RShift(lineMatch.Groups[1].Value, int32 lineMatch.Groups[2].Value, lineMatch.Groups[3].Value)
        else if notRegex.IsMatch(line) then 
            let lineMatch = notRegex.Match(line)
            Not(lineMatch.Groups[1].Value, lineMatch.Groups[2].Value)
        else
            failwith "Unknown Instruction"
            

    File.ReadLines(filepath)
    |> Seq.map mapInstruction
    |> Seq.toList

let parseUsableInstructions (valueMap: Map<string, int>) (instructions: Instruction list) =
    let rec parseInstructions (remaining: Instruction list) (usable: Instruction list) (unusable: Instruction list) = 
        if List.isEmpty remaining then 
            (usable, unusable)
        else
            let nextInstruction = List.head remaining
            let instructionUsable = 
                match nextInstruction with 
                | Assign(source, _) when Map.containsKey source valueMap -> true
                | Assign(assignValue, _) when 
                    let success, value = System.Int32.TryParse(assignValue)
                    success
                    -> true
                | And(source1, source2, _) when Map.containsKey source1 valueMap && Map.containsKey source2 valueMap -> true
                | Or(source1, source2, _) when Map.containsKey source1 valueMap && Map.containsKey source2 valueMap -> true
                | LShift(source, _, _) when Map.containsKey source valueMap -> true
                | RShift(source, _, _) when Map.containsKey source valueMap -> true
                | Not(source, _) when Map.containsKey source valueMap -> true
                | _ -> false
            if instructionUsable then 
                parseInstructions (List.tail remaining) (nextInstruction :: usable) (unusable)
            else    
                parseInstructions (List.tail remaining) (usable) (nextInstruction :: unusable)
            
    parseInstructions instructions List.empty List.empty

let Part1 (input: Instruction list) =
    let rec processInstructions (remaining: Instruction list) (registerValues: Map<string, int>) = 
        let instructionFold (acc: Map<string,int>) (nextInstruction: Instruction) = 
            match nextInstruction with 
            | Assign(source, target) ->
                let isInt, sourceValue = System.Int32.TryParse(source)
                if isInt then 
                    Map.add target sourceValue acc
                else
                    let sourceMapValue = Map.find source acc
                    Map.add target sourceMapValue acc
            | And(source1, source2, target) ->
                let source1Value = int32 (Map.find source1 acc)
                let source2Value = int32 (Map.find source2 acc)
                let targetValue = source1Value &&& source2Value
                Map.add target targetValue acc
            | Or(source1, source2, target) -> 
                let source1Value = int32 (Map.find source1 acc)
                let source2Value = int32 (Map.find source2 acc)
                let targetValue = source1Value ||| source2Value
                Map.add target targetValue acc
            | LShift(source, shiftValue, target) ->
                let sourceValue = Map.find source acc
                let targetValue = sourceValue <<< shiftValue
                Map.add target targetValue acc
            | RShift(source, shiftValue, target) -> 
                let sourceValue = Map.find source acc
                let targetValue = sourceValue >>> shiftValue
                Map.add target targetValue acc
            | Not(source, target) -> 
                let sourceValue = Map.find source acc
                let targetValue = ~~~ sourceValue 
                Map.add target targetValue acc


        if List.isEmpty remaining then 
            registerValues
        else
            let (usableInstructions, nonUsableInstructions) = parseUsableInstructions registerValues remaining
            let nextRegisterValues = 
                usableInstructions
                |> List.fold instructionFold registerValues
            processInstructions nonUsableInstructions nextRegisterValues
    
    let registerMap = processInstructions input Map.empty
    Map.find "a" registerMap

let Part2 input = 
    0