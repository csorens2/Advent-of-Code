module Day6

open System.IO

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.head
    |> Seq.toArray

let rec SignalSeq charMap signalChars currPos signalLength = seq {
    if currPos < Array.length signalChars then 
        let nextChar = signalChars.[currPos]
        let charMapAdded = 
            match Map.tryFind nextChar charMap with 
            | None -> Map.add nextChar 1 charMap
            | Some(lastVal) -> Map.add nextChar (lastVal + 1) charMap
        let charMapFinal = 
            if currPos < signalLength then 
                charMapAdded
            else
                let toRemove = signalChars.[currPos - signalLength]
                match Map.tryFind toRemove charMapAdded with 
                | None -> failwith "There should be 1 present to remove"
                | Some(lastVal) when lastVal = 1 -> Map.remove toRemove charMapAdded
                | Some(lastVal) -> Map.add toRemove (lastVal - 1) charMapAdded

        yield (charMapFinal, currPos)
        yield! (SignalSeq charMapFinal signalChars (currPos + 1) signalLength)
}

let Part1 (input: char array) = 
    let sequenceLength = 4

    let (_, markerPosition) = 
        SignalSeq Map.empty input 0 sequenceLength
        |> Seq.skipWhile (fun (charMap, _) -> (Seq.length (Map.keys charMap)) <> sequenceLength)
        |> Seq.head
    
    (markerPosition + 1)

let Part2 input = 
    let sequenceLength = 14

    let (_, markerPosition) = 
        SignalSeq Map.empty input 0 sequenceLength
        |> Seq.skipWhile (fun (charMap, _) -> (Seq.length (Map.keys charMap)) <> sequenceLength)
        |> Seq.head
    
    (markerPosition + 1)