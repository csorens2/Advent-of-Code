module Day7

open System.IO
open System.Text.RegularExpressions

type Bridge = {
    Total: int64
    Nums: int64 list
}

let ParseInput filepath = 
    let parseLine (line: string) =
        let splitString = line.Split ':'
        let total = int64 splitString[0]
        let nums = 
            (Regex("(\d+)").Matches splitString[1])
            |> Seq.map (fun lineMatch -> int64 lineMatch.Groups[1].Value)
            |> Seq.toList
        {Bridge.Total = total; Nums = nums}

    File.ReadLines(filepath)
    |> Seq.map parseLine
    |> Seq.toList

let Part1 input = 
    let processBridge bridge = 
        let rec processNums (remainingNums: int64 list) total currTotal = 
            if remainingNums.IsEmpty then 
                if total = currTotal then 
                    true
                else 
                    false
            else if currTotal > total then 
                false
            else
                let nextNum = remainingNums.Head
                let addNum = processNums remainingNums.Tail total (currTotal + nextNum)
                let mulNum = processNums remainingNums.Tail total (currTotal * nextNum)
                addNum || mulNum
        processNums bridge.Nums.Tail bridge.Total bridge.Nums.Head

    input
    |> List.filter processBridge
    |> List.map (fun bridge -> bridge.Total)
    |> List.sum

let Part2 input = 
    let processBridge bridge = 
        let rec processNums (remainingNums: int64 list) initialNum (currTotal: int64) =
            if remainingNums.IsEmpty then
                if initialNum = currTotal then 
                    true
                else
                    false
            else if currTotal < initialNum then 
                false
            else
                let nextNum = remainingNums.Head
                let divideSuccess = 
                    if currTotal % nextNum = 0 then 
                        processNums remainingNums.Tail initialNum (currTotal / nextNum)
                    else
                        false
                let subtractSuccess = 
                    processNums remainingNums.Tail initialNum (currTotal - nextNum)
                let concatSuccess = 
                    let currTotalString = currTotal.ToString()
                    let nextNumString = nextNum.ToString()
                    if currTotalString.EndsWith(nextNumString) then 
                        let unConcatedString = currTotalString.Substring(0, currTotalString.Length - nextNumString.Length)
                        if unConcatedString = "" then 
                            false
                        else
                            processNums remainingNums.Tail initialNum (int64 unConcatedString)
                    else
                        false
                divideSuccess || subtractSuccess || concatSuccess

        processNums (List.rev bridge.Nums.Tail) bridge.Nums.Head bridge.Total

    input
    |> List.filter processBridge
    |> List.map (fun bridge -> bridge.Total)
    |> List.sum