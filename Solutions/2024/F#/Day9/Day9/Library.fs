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

let CalculateChecksum (diskMap: DiskMapSpace PersistentVector) = 

    let checksumFolder acc nextIndex = 
        match diskMap[nextIndex] with 
        | File(id) -> acc + ((int64 nextIndex) * id)
        | FreeSpace -> acc

    diskMap
    |> PersistentVector.mapi (fun i _ -> i)
    |> PersistentVector.fold checksumFolder 0L

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
    
    CalculateChecksum (moveBlocks input 0 (input.Length - 1))


let Part2 (input: PersistentVector<DiskMapSpace>) = 

    let rec buildFreespaceMap (freespaceMap: Map<int, IPriorityQueue<int>>) currIndex = 
        if PersistentVector.length input <= currIndex then 
            freespaceMap
        else
            if input[currIndex].IsFile then 
                buildFreespaceMap freespaceMap (currIndex + 1)
            else
                let rec countFreespace freespaceIndex = 
                    if freespaceIndex = input.Length then 
                        0
                    else if input[freespaceIndex].IsFile then 
                        0
                    else
                        1 + countFreespace (freespaceIndex + 1)
                let freeSpaceLength = countFreespace currIndex

                let nextMap = 
                    let heapToUpdate = 
                        match Map.tryFind freeSpaceLength freespaceMap with 
                        | Some(prevHeap) -> prevHeap
                        | None -> PriorityQueue.empty false
                    Map.add freeSpaceLength (PriorityQueue.insert currIndex heapToUpdate) freespaceMap
                buildFreespaceMap (nextMap) (currIndex + freeSpaceLength)
                    
    let rec processFiles (currVector: PersistentVector<DiskMapSpace>) (currFreespaceMap: Map<int, IPriorityQueue<int>>) currIndex = 
        if currIndex <= 0 then 
            currVector
        else
            match currVector[currIndex] with 
            | FreeSpace -> processFiles currVector currFreespaceMap (currIndex - 1)
            | File(currID) -> 
                let rec countFileSpace countIndex = 
                    if countIndex < 0 then 
                        0
                    else
                        match currVector[countIndex] with 
                        | FreeSpace -> 0
                        | File(countID) when currID <> countID -> 0
                        | File(_) -> 1 + countFileSpace (countIndex - 1)

                let fileSpace = countFileSpace currIndex

                let possibleFreeSpaceLengths = 
                    currFreespaceMap
                    |> Map.keys
                    |> Seq.filter (fun freespaceSize -> fileSpace <= freespaceSize)

                if Seq.isEmpty possibleFreeSpaceLengths then 
                    processFiles currVector currFreespaceMap (currIndex - fileSpace)
                else
                    let possibleLeftMost = 
                        possibleFreeSpaceLengths
                        |> Seq.map (fun length -> (currFreespaceMap[length].Peek(), length))
                        |> Seq.filter (fun (pos, _) -> pos < currIndex)
                        |> Seq.sortBy (fun (pos, _) -> pos)
                        |> Seq.tryHead

                    if possibleLeftMost.IsNone then 
                        processFiles currVector currFreespaceMap (currIndex - fileSpace)
                    else
                        let (leftMostSpace, leftMostLength) = possibleLeftMost.Value

                        let foldVector acc nextStep = 
                            acc
                            |> PersistentVector.update (leftMostSpace + nextStep) (currVector[currIndex - nextStep])
                            |> PersistentVector.update (currIndex - nextStep) (currVector[leftMostSpace + nextStep])

                        let nextVector = List.fold foldVector currVector [0..(fileSpace-1)]

                        let nextFreespaceMap = 
                            let heapWithLeftMostSpaceRemoved =
                                let (_, toReturn) = currFreespaceMap[leftMostLength].Pop()
                                toReturn
  
                            let mapWithFreespaceRemoved = 
                                if heapWithLeftMostSpaceRemoved.Count = 0 then 
                                    Map.remove leftMostLength currFreespaceMap
                                else
                                    Map.add leftMostLength (heapWithLeftMostSpaceRemoved) currFreespaceMap

                            if fileSpace = leftMostLength then 
                                mapWithFreespaceRemoved
                            else // If the file doesn't fit completely in the free space, we need to add the remaining free space back to the freespace map
                                let updatedHeap = currFreespaceMap[leftMostLength - fileSpace].Insert (leftMostSpace + fileSpace)
                                Map.add (leftMostLength - fileSpace) updatedHeap mapWithFreespaceRemoved

                        processFiles nextVector nextFreespaceMap (currIndex - fileSpace)
    
    CalculateChecksum (processFiles input (buildFreespaceMap Map.empty 0 ) (input.Length - 1))