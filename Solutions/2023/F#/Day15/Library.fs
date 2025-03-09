module Day15

open System.IO

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.head

let Part1 (input:string) = 
    let rec processHash (prevValue: int) (remainingString: char List) = 
        if remainingString.IsEmpty then 
            prevValue
        else
            let nextASCII = int remainingString.Head
            let nextValue = (((prevValue + nextASCII) * 17) % 256)
            processHash nextValue remainingString.Tail
 
    input.Split(',')
    |> Array.map (fun seqString -> processHash 0 (Seq.toList seqString))
    |> Array.sum

let Part2 input = 
    0