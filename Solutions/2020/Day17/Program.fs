open System
open System.IO
open FSharp.Collections
open System.Diagnostics

/// The problem deals with "Conway Cubes". But seeing as its only a cube in problem 1, and in problem 2 its a tesseract, we'll just call them "Conways"
///
/// Yes, I am aware that the dimensions then are just another name for vector. I like keeping things practical instead of academic.
type Conway = {
    Coordinates: int array
}

type Version = 
    | Original
    | Optimized

type Problem = 
    | One
    | Two

type SimulationParameters = {
    Part: Problem
    SimulationVersion: Version
    CycleFunction: Set<Conway> -> Set<Conway>
    Dimensions: int
    Cycles: int
    
}


(*********************)
(* Original Solution *)
(*********************)
let GetNumDimensions conSet = 
    match conSet |> Set.toSeq |> Seq.tryHead with 
    | Some con -> con.Coordinates.Length
    | None -> raise (Exception "No conways in set.") 

/// Gets the bounds for the space that the given conways take up. For example, the (min,max)x, (min,max)y, etc.
let GetBoundsFromConwaySet conSet = 
    Array.init (GetNumDimensions conSet) (fun dimensionIndex -> 
        let dimensionValues = 
            conSet
            |> Set.toSeq
            |> Seq.map (fun con -> con.Coordinates[dimensionIndex])
        (Seq.min dimensionValues, Seq.max dimensionValues))

/// Gets all conways located in a bounds
let rec GetAllConwaysInBounds (currentDimensionValues: int array) dimensionIndex (bounds: (int*int) array) = seq {
    match dimensionIndex = currentDimensionValues.Length with 
    | true -> 
        yield {Conway.Coordinates = currentDimensionValues}
    | false -> 
        let (leftBound,rightBound) = bounds[dimensionIndex]
        for nextDimValue in [leftBound..rightBound] do
            yield! 
                GetAllConwaysInBounds
                    (Array.mapi (fun index value -> if index = dimensionIndex then nextDimValue else value) currentDimensionValues)
                    (dimensionIndex + 1)
                    bounds
}

/// Counts the number of conways in the given set that surround the given conway
let CountSurrounding targetCon conSet = 
    GetBoundsFromConwaySet (Set.add targetCon Set.empty)
    |> Array.map (fun (min,max) -> (min-1,max+1))
    |> GetAllConwaysInBounds (Array.create targetCon.Coordinates.Length 0) 0
    |> Set.ofSeq
    |> Set.remove targetCon
    |> Set.intersect conSet
    |> Set.count   

/// Process a single cycle
let ProcessCycle conSet = 
    GetBoundsFromConwaySet conSet
    |> Array.map (fun (min,max) -> (min-1,max+1))
    |> GetAllConwaysInBounds (Array.create (GetNumDimensions conSet) 0) 0
    |> Set.ofSeq
    |> Set.filter (fun con ->
        match Set.contains con conSet with
        | true -> // Con is active
            match CountSurrounding con conSet with
            | 2 | 3 -> true
            | _ -> false
        | false -> // Con is inactive
            match CountSurrounding con conSet with
            | 3 -> true
            | _ -> false
        )

(* Optimized Version *)
(* 1. Instead of defining an area containing all the active and inactive conways, then processing them all to find the actives, we instead only find the inactive conways that are next to active ones, and process them and the active ones.*)
(* 2. During the active-check, instead of counting all the surrounding conways that are active then checking if its one of the valid values, we end the count early and return false if we surpass all of the valid values.*)  
let rec GetSurroundingConways (currentDimensionValues: int array) dimensionIndex centerCon = seq {
    match dimensionIndex = currentDimensionValues.Length with 
    | true -> 
        if currentDimensionValues <> centerCon.Coordinates then
            yield {Conway.Coordinates = currentDimensionValues}
    | false -> 
        for nextDimValue in [centerCon.Coordinates[dimensionIndex]-1..centerCon.Coordinates[dimensionIndex]+1] do
            yield! 
                GetSurroundingConways
                    (Array.mapi (fun index value -> if index = dimensionIndex then nextDimValue else value) currentDimensionValues)
                    (dimensionIndex + 1)
                    centerCon
}

let rec GetInactiveConways activeCons = 
    activeCons
    |> Set.toSeq
    |> Seq.collect (fun activeCon -> 
        let surroundingCons = 
            GetSurroundingConways (Array.create activeCon.Coordinates.Length 0) 0 activeCon
            |> Set.ofSeq
        Set.difference surroundingCons activeCons)
    |> Set.ofSeq

///
/// Count the surrounding Conways until one of the following:
///
/// 1. You run out of Conways to count, then you just check if the number is valid.
/// 
/// 2. Your count becomes greater than any of the valid counts.
///
/// This is seperate from "ConwayIsActive" to allow for early stopping.
///
let rec RecConwayIsActive surroundingCons activeCons currCount validCounts = 
    match List.isEmpty surroundingCons with 
    | true -> 
        List.contains currCount validCounts // Stop counting when the list is done, and check if we got one of the conditions
    | false -> 
        let nextCount = 
            currCount + 
            match Set.contains (List.head surroundingCons) activeCons with 
            | true ->
                1
            | false ->
                0
        let prunedValids = validCounts |> List.where (fun validCount -> nextCount <= validCount)
        match List.isEmpty prunedValids with 
        | true -> false // Stop counting if we surpassed all the valid counts
        | false -> RecConwayIsActive (List.tail surroundingCons) activeCons nextCount prunedValids 
            
            
let ConwayIsActive activeCons toCheck activeCount = 
    RecConwayIsActive
        (Seq.toList (GetSurroundingConways (Array.create toCheck.Coordinates.Length 0) 0 toCheck))
        activeCons
        0
        activeCount

let ProcessCycleOptimized activeCons = 
    let inactiveToActive = 
        (GetInactiveConways activeCons)
        |> Set.filter (fun inactiveCon ->
            ConwayIsActive activeCons inactiveCon [3])
    let stayActive = 
        activeCons
        |> Set.filter (fun activeCon ->
            ConwayIsActive activeCons activeCon [2;3])
    Set.union inactiveToActive stayActive

(********************)
(* Shared Functions *)
(********************)
/// Scales the Conway's dimensions. For example, going from 3d to 4d, or 4d to 2d.
let ScaleConwayDimensions targetDimensions conSet = 
    let baseDimensions = GetNumDimensions conSet
    let dimensionDiff = targetDimensions - baseDimensions
    conSet
    |> Set.map (fun con -> 
        {
            con with
                Coordinates = 
                    match sign dimensionDiff with
                    | 1 -> Array.append con.Coordinates (Array.init dimensionDiff (fun _ -> 0)) // Grow the dimensions
                    | -1 -> Array.take (abs dimensionDiff) con.Coordinates // Shrink the dimensions
                    | _ -> con.Coordinates
        })

let rec RunCycles cycleFunc remainingCycles conSet = 
    match remainingCycles with 
    | 0 -> conSet
    | _ -> RunCycles cycleFunc (remainingCycles - 1) (cycleFunc conSet) 

let RunSimulation initialConways (parameter: SimulationParameters) = 
    let stopwatch = Stopwatch.StartNew()
    let simResult = 
        RunCycles 
            parameter.CycleFunction 
            parameter.Cycles 
            (ScaleConwayDimensions parameter.Dimensions initialConways)
    printfn "Part %s %s Result: %d Runtime: %.2f" 
        (parameter.Part.ToString())
        (parameter.SimulationVersion.ToString())
        (Set.count simResult)
        (stopwatch.Elapsed.TotalSeconds)      

let ParseInput filepath = 
    let activeSpace = '#'
    File.ReadLines(filepath)
    |> Seq.indexed
    |> Seq.map (fun (yPos, line) -> 
        line
        |> Seq.indexed
        |> Seq.where (fun (_, conValue) -> conValue = activeSpace)
        |> Seq.map (fun (xPos, _ ) -> {Conway.Coordinates = [|xPos;yPos|]}))
    |> Seq.collect id
    |> Set.ofSeq   

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let SimulationList = [
        {SimulationParameters.Part = Problem.One; SimulationVersion = Version.Original; SimulationParameters.CycleFunction = ProcessCycle; Dimensions = 3; Cycles = 6}
        {SimulationParameters.Part = Problem.One; SimulationVersion = Version.Optimized; SimulationParameters.CycleFunction = ProcessCycleOptimized; Dimensions = 3; Cycles = 6}
        {SimulationParameters.Part = Problem.Two; SimulationVersion = Version.Original; SimulationParameters.CycleFunction = ProcessCycle; Dimensions = 4; Cycles = 6}
        {SimulationParameters.Part = Problem.Two; SimulationVersion = Version.Optimized; SimulationParameters.CycleFunction = ProcessCycleOptimized; Dimensions = 4; Cycles = 6}
    ]
    // Part 1: 359
    // Part 2: 2228
    for simulation in SimulationList do
        RunSimulation input simulation
    0