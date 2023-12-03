module Day1

open System
open System.IO

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.toList

let rec BuildNumSeq_Digit remainingString = 
    remainingString
    |> Seq.filter (fun character -> System.Char.IsDigit(character))
    |> Seq.map (fun toConvert -> (int toConvert) - (int '0'))
    
let rec BuildNumSeq_NameAndDigit remainingString = seq {
    let numsList = [
        ("one", 1)
        ("two", 2)
        ("three", 3)
        ("four", 4)
        ("five", 5)
        ("six", 6)
        ("seven", 7)
        ("eight", 8)
        ("nine", 9)
    ]

    if not (String.IsNullOrEmpty remainingString) then
        match System.Char.IsDigit (Seq.head remainingString) with
        | true -> 
            yield (int (Seq.head remainingString)) - (int '0')
            yield! BuildNumSeq_NameAndDigit (remainingString.Substring 1)
        | false -> 
            let numMatch = List.tryFind (fun ((numString:string), _) -> remainingString.StartsWith numString) numsList
            match numMatch with 
            | Some (numString, numVal) -> 
                yield numVal
                yield! BuildNumSeq_NameAndDigit (remainingString.Substring (numString.Length - 1))
            | None ->
                yield! BuildNumSeq_NameAndDigit (remainingString.Substring 1)       
}

let CalibrationSum input amendedFunction = 
    input
    |> List.map (fun amendedCalibration ->
        let intList = 
            amendedFunction amendedCalibration
            |> Seq.toList
        match List.isEmpty intList with
        | true -> 0
        | false -> (10 * (List.head intList)) + (List.last intList))           
    |> List.sum

let Part1 input = 
    CalibrationSum input BuildNumSeq_Digit

let Part2 input = 
    CalibrationSum input BuildNumSeq_NameAndDigit

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 52974
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 53340
    0