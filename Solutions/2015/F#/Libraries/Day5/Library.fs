module Day5

open System.IO

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.toList

let Part1 input =         
    let vowelCheck (toCheck: string) = 
        let countCharOccurance charToCount = 
            toCheck
            |> String.filter(fun nextChar -> nextChar = charToCount)
            |> String.length

        let vowelCount = 
            ['a'; 'e'; 'i'; 'o'; 'u']
            |> List.map countCharOccurance
            |> List.sum
        if vowelCount >= 3 then 
            true
        else
            false

    let doubleCheck (toCheck: string) = 
        ['a'..'z']
        |> List.exists (fun nextChar -> toCheck.Contains((string nextChar) + (string nextChar)))

    let flaggedCheck (toCheck: string) = 
        let flaggedCount = 
            ["ab"; "cd"; "pq"; "xy"]
            |> List.filter (fun nextString -> toCheck.Contains(nextString))
            |> List.length
        if flaggedCount = 0 then 
            true
        else
            false
            
    input
    |> List.filter (fun line -> vowelCheck line && doubleCheck line && flaggedCheck line)
    |> List.length

let Part2 input = 
    let check1 (toCheck: string) = 
        let checkCharPair charPair =
            if toCheck.Length - (toCheck.Replace(charPair, "").Length) >= 4 then 
                true
            else
                false
                
        let charPairs = seq {
            for i in 'a'..'z' do 
                for j in 'a'..'z' do 
                    yield (string i) + (string j)
        }

        charPairs
        |> Seq.exists (fun nextPair -> checkCharPair nextPair)

    let check2 (toCheck: string) = 
        let repeatBetweenStrings = seq {
            for i in 'a'..'z' do 
                for j in 'a'..'z' do 
                    yield (string i) + (string j) + (string i)
        }

        repeatBetweenStrings
        |> Seq.exists (fun nextString -> toCheck.Contains(nextString))

    input
    |> List.filter (fun nextString -> check1 nextString && check2 nextString)
    |> List.length