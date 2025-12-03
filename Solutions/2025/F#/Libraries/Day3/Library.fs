module Day3

open System.IO

let ParseInput filepath = 
    let parseLine line = 
        line
        |> Seq.map (fun charNum -> int64 charNum - int64 '0')
        |> Seq.toList

    File.ReadLines(filepath)
    |> Seq.map parseLine
    |> Seq.toList

let rec GenerateBatteryNum remainingBatteries numRemainingSelections = 
    if numRemainingSelections = 0 then 
        0L
    else
        let selectionRangeLength = (List.length remainingBatteries) - numRemainingSelections
            
        let possibleBatteriesWithIndex = 
            remainingBatteries
            |> List.take (selectionRangeLength+1)
            |> List.indexed
            
        let (_, maxJoltage) = 
            possibleBatteriesWithIndex
            |> List.maxBy (fun (_, joltage) -> joltage)

        let (leftmostIndex, _) = 
            possibleBatteriesWithIndex
            |> List.find (fun (_, joltage) -> joltage = maxJoltage)

        let addedValue = maxJoltage * (pown 10L (numRemainingSelections-1))
            
        let nextRemainingBatteries = List.skip (leftmostIndex + 1) remainingBatteries
            
        addedValue + GenerateBatteryNum nextRemainingBatteries (numRemainingSelections - 1)

let Part1 input = 
    input
    |> List.map (fun batteryList -> GenerateBatteryNum batteryList 2)
    |> List.sum

let Part2 input = 
    input
    |> List.map (fun batteryList -> GenerateBatteryNum batteryList 12)
    |> List.sum

let Part1OLD input = 
    let rec processBatteryList batteryList =
        let batteryArray = List.toArray batteryList

        let rec buildMaxMap index = 
            if index = (Array.length batteryArray - 1) then 
                Map.add index batteryArray[index] Map.empty
            else
                let currBattery = batteryList[index]
                let laterMap = buildMaxMap (index + 1)

                let maxBattery = max (currBattery) (Map.find (index + 1) laterMap)

                Map.add index maxBattery laterMap
        
        let maxMap = buildMaxMap 0

        let maxBatteryNonEnd = 
            batteryList
            |> List.rev
            |> List.tail
            |> List.max

        let processIndexJoltagePair (index, joltage) = 
            let maxRight = Map.find (index + 1) maxMap
            (joltage * 10L) + maxRight

        batteryList
        |> List.indexed
        |> List.rev
        |> List.tail
        |> List.filter (fun (_, joltage) -> joltage = maxBatteryNonEnd)
        |> List.map processIndexJoltagePair
        |> List.max

    input
    |> List.map processBatteryList
    |> List.sum