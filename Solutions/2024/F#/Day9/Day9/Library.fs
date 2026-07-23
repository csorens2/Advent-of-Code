module Day9

open System.IO
open FSharpx.Collections

type DiskMapSpace = 
    | File of ID: int64
    | FreeSpace

let ParseInput filepath = 
    let rec buildDiskMapVector remainingNums isFile currFileID finalList = 
        if List.isEmpty remainingNums then 
            finalList
            |> List.rev
            |> PersistentVector.ofSeq
        else
            let nextNum = remainingNums.Head 

            let nextListFolder = 
                if isFile then 
                    fun acc _ -> File(currFileID) :: acc
                else
                    fun acc _ -> FreeSpace :: acc
            let nextList = 
                [0..(nextNum-1)]
                |> List.fold nextListFolder finalList

            let nextID = 
                if isFile then 
                    currFileID + 1L
                else
                    currFileID

            buildDiskMapVector remainingNums.Tail (not isFile) nextID nextList
            
    let baseList = 
        File.ReadLines(filepath)
        |> Seq.head
        |> Seq.map (fun intchar -> int intchar - int '0')
        |> Seq.toList

    buildDiskMapVector baseList true 0 List.empty

let Part1 input = 
    
    let rec moveBlocks (currVector: DiskMapSpace PersistentVector) leftIndex rightIndex = 
        if leftIndex = rightIndex then 
            currVector
        else
            if currVector[leftIndex].IsFile then 
                moveBlocks currVector (leftIndex + 1) rightIndex
            else if currVector[rightIndex].IsFreeSpace then 
                moveBlocks currVector leftIndex (rightIndex - 1) 
            else 
                let nextArray = 
                    currVector
                    |> PersistentVector.update leftIndex currVector[rightIndex]
                    |> PersistentVector.update rightIndex currVector[leftIndex]
                moveBlocks nextArray (leftIndex + 1) (rightIndex - 1)
    
    let movedVector = moveBlocks input 0 (input.Length - 1)

    let checksumFolder acc nextIndex = 
        match movedVector[nextIndex] with 
        | File(id) -> acc + ((int64 nextIndex) * id)
        | FreeSpace -> acc

    movedVector
    |> PersistentVector.mapi (fun i _ -> i)
    |> PersistentVector.fold checksumFolder 0L


let Part2 input = 
    0