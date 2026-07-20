module Day9

open System.IO

type DiskMapEntry = 
    | File of ID: int * Length: int
    | FreeSpace of Length: int

let ParseInput filepath = 
    let rec BuildEntryList remainingNums isFile currFileID finalEntryList = 
        if List.isEmpty remainingNums then 
            finalEntryList
        else
            let nextNum = remainingNums.Head
            let nextEntry = 
                if isFile then 
                    File(currFileID, nextNum)
                else
                    FreeSpace(nextNum)
            let nextID = 
                if isFile then 
                    (currFileID + 1) % 10
                else
                    currFileID
            BuildEntryList remainingNums.Tail (not isFile) nextID (nextEntry :: finalEntryList)
    
    let baseList = 
        File.ReadLines(filepath)
        |> Seq.head
        |> Seq.map (fun intchar -> int intchar - int '0')
        |> Seq.toList

    BuildEntryList baseList true 0 List.empty
    |> List.rev
    |> List.toArray


let Part1 input = 

    let rec processArray remainingArray leftIndex rightIndex finalFileList = 
        if rightIndex = leftIndex then 
            match Array.get remainingArray leftIndex with 
            | File(id, length) -> File(id, length) :: finalFileList
            | FreeSpace(_) -> finalFileList
        else    
            let nextLeft = remainingArray[leftIndex]



    0

let Part2 input = 
    0