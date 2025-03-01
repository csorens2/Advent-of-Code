module Day11

open System.IO
open System.Text.RegularExpressions

let ParseInput filepath = 
    let parseLine (line: string) = 
        Regex("(\d+)").Matches line
        |> Seq.map (fun nextMatch -> nextMatch.Groups[0].Value)

    File.ReadLines(filepath)
    |> Seq.head
    |> parseLine

let rec ProcessNum num remainingBlinks (memoization: Map<(string*int), int64>) = 
    if memoization.ContainsKey (num, remainingBlinks) then 
        memoization
    else
        if remainingBlinks = 0 then 
            Map.add (num, 0) 1 memoization
        else if num = "0" then 
            let subMap = ProcessNum "1" (remainingBlinks - 1) memoization
            Map.add (num, remainingBlinks) (Map.find ("1", remainingBlinks - 1) subMap) subMap
        else if num.Length % 2 = 0 then
            let leftNum = (int64 (num.Substring(0, num.Length / 2))).ToString()
            let rightNum = (int64 (num.Substring(num.Length / 2))).ToString()
            let leftMemo = ProcessNum leftNum (remainingBlinks - 1) memoization
            let rightMemo = ProcessNum rightNum (remainingBlinks - 1) leftMemo
            let sumLeftRight = 
                (Map.find (leftNum, remainingBlinks - 1) rightMemo) + 
                (Map.find (rightNum, remainingBlinks - 1) rightMemo) 
            Map.add (num, remainingBlinks) sumLeftRight rightMemo
        else 
            let nextNum = ((int64 num) * 2024L).ToString()
            let submap = ProcessNum nextNum (remainingBlinks - 1) memoization
            Map.add (num, remainingBlinks) (Map.find (nextNum, remainingBlinks - 1) submap) submap

let ProcessNums numBlinks nums = 
    let memoMap = 
        nums
        |> Seq.fold (fun acc nextNum -> ProcessNum nextNum numBlinks acc) Map.empty
    
    nums
    |> Seq.fold (fun acc nextNum -> acc + (Map.find (nextNum, numBlinks) memoMap)) 0L

let Part1 input = 
    ProcessNums 25 input

let Part2 input = 
    ProcessNums 75 input

    

    
            

                
            
                
            
    