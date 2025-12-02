module Day1

open System.IO
open System.Text.RegularExpressions

type Direction = 
    | Left
    | Right

type Rotation = {
    Direction: Direction
    Amount: int
}

let ParseInput filepath = 
    let parseLine line = 
        let regexMatch = Regex(@"([L|R])(\d+)").Match(line)
        let direction = 
            if regexMatch.Groups[1].Value.ToUpper() = "R" then 
                Right
            else 
                Left
        let amount = int regexMatch.Groups[2].Value
        {Rotation.Direction = direction; Amount = amount}

    File.ReadLines(filepath)
    |> Seq.map parseLine
    |> Seq.toList

let Part1 input = 
    let rotationsFolder acc nextRotation = 
        let rotationAmount = 
            match nextRotation.Direction with 
            | Left -> -1 * nextRotation.Amount
            | Right -> nextRotation.Amount

        (((acc + rotationAmount) % 100) + 100) % 100

    List.scan rotationsFolder 50 input
    |> List.sumBy(fun pos -> if pos = 0 then 1 else 0)

let Part2 input = 
    
    let rec performRotations remainingRotations numZeros currPos = 
        if List.isEmpty remainingRotations then 
            numZeros
        else
            let nextRotation = List.head remainingRotations
            if nextRotation.Amount >= 100 then 
               let remainingFront = {Rotation.Amount = nextRotation.Amount - 100; Direction = nextRotation.Direction}
               let nextRemaining = remainingFront :: (List.tail remainingRotations)
               performRotations nextRemaining (numZeros + 1) currPos
            else 
                let nextPosRaw = 
                    match nextRotation.Direction with 
                    | Left -> currPos - nextRotation.Amount
                    | Right -> currPos + nextRotation.Amount
                if nextPosRaw = 0 then 
                    performRotations (List.tail remainingRotations) (numZeros + 1) nextPosRaw
                else if nextPosRaw < 0 && not (currPos = 0) then
                    performRotations (List.tail remainingRotations) (numZeros + 1) (nextPosRaw + 100)
                else if nextPosRaw < 0 && (currPos = 0) then 
                    performRotations (List.tail remainingRotations) (numZeros) (nextPosRaw + 100)
                else if nextPosRaw >= 100 then 
                    performRotations (List.tail remainingRotations) (numZeros + 1) (nextPosRaw - 100)
                else
                    performRotations (List.tail remainingRotations) numZeros nextPosRaw

    performRotations input 0 50
    


    