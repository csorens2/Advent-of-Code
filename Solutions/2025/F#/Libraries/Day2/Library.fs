module Day2

open System.IO
open System.Text.RegularExpressions

let ParseInput filepath =
    let parseRange range = 
        let regexMatch = Regex(@"(\d+)-(\d+)").Match(range)
        (int64 regexMatch.Groups[1].Value, int64 regexMatch.Groups[2].Value)
    
    let line = 
        File.ReadLines(filepath)
        |> Seq.head

    line.Split [|','|]
    |> Array.map parseRange
    |> Array.toList


let Part1 (input: (int64*int64) list) = 
    let processRange (rangeLeft, rangeRight) = 
        seq {
            for num in rangeLeft..rangeRight do 
                let numString = string num 
                let numLength = String.length numString
                if numLength % 2 = 0 then 
                    let leftString = numString.Substring(0, numLength / 2)
                    let rightString = numString.Substring(numLength / 2)
                    if leftString = rightString then 
                        yield num
        }
    
    input 
    |> List.map processRange
    |> List.sumBy (fun invalidIds -> Seq.sum invalidIds)

let Part2 (input: (int64*int64) list) = 
    let processRange (rangeLeft, rangeRight) = 
        seq {
            for num in rangeLeft..rangeRight do 
                let numString = string num 
                let numLength = String.length numString

                let checkChunkLength chunkLength = 
                    if numLength % chunkLength <> 0 then 
                        false
                    else if chunkLength = numLength then 
                        false
                    else
                        let numChunkArrays = Seq.chunkBySize chunkLength numString
                        let numStrings = Seq.map (fun (chunkArray: char array) -> System.String chunkArray) numChunkArrays
                        let toCheck = Seq.head numStrings
                        if Seq.forall (fun chunkToCheck -> toCheck = chunkToCheck) numStrings then 
                            true
                        else 
                            false
                
                let idInvalid = 
                    [1..numLength]
                    |> List.map checkChunkLength
                    |> List.filter id
                    |> List.isEmpty
                    |> not

                if idInvalid = true then 
                    yield num                       
        }

    input 
    |> List.map processRange
    |> List.sumBy (fun invalidIds -> Seq.sum invalidIds)