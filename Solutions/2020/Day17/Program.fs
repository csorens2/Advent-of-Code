open System
open System.IO

/// The problem deals with "Conway Cubes". But seeing as its only a cube in problem 1, and in problem 2 its a tesseract, we'll just call them "Conways"
///
/// Yes, I am aware that the dimensions then are just another name for vector. I like keeping things practical instead of academic.
type Conway = {
    Dimensions: int array
}

let ParseInput filepath = 
    let activeSpace = '#'
    File.ReadLines(filepath)
    |> Seq.indexed
    |> Seq.map (fun (yPos, line) -> 
        line
        |> Seq.indexed
        |> Seq.where (fun (_, conValue) -> conValue = activeSpace)
        |> Seq.map (fun (xPos, _ ) -> {Conway.Dimensions = [|xPos;yPos|]}))
    |> Seq.collect id
    |> Set.ofSeq    

let GetNumDimensions conSet = 
    match conSet |> Set.toSeq |> Seq.tryHead with 
    | Some con -> con.Dimensions.Length
    | None -> raise (Exception "No conways in set.") 

/// Scales the Conway's dimensions. For example, going from 3d to 4d, or 4d to 2d.
let ScaleConwayDimensions targetDimensions conSet = 
    let baseDimensions = GetNumDimensions conSet
    let dimensionDiff = targetDimensions - baseDimensions
    conSet
    |> Set.map (fun con -> 
        {
            con with
                Dimensions = 
                    match sign dimensionDiff with
                    | 1 -> Array.append con.Dimensions (Array.init dimensionDiff (fun _ -> 0)) // Grow the dimensions
                    | -1 -> Array.take (abs dimensionDiff) con.Dimensions // Shrink the dimensions
                    | _ -> con.Dimensions
        })

/// Gets the bounds for the space that the given conways take up. For example, the (min,max)x, (min,max)y, etc.
let GetBoundsFromConwaySet conSet = 
    Array.init (GetNumDimensions conSet) (fun dimensionIndex -> 
        let dimensionValues = 
            conSet
            |> Set.toSeq
            |> Seq.map (fun con -> con.Dimensions[dimensionIndex])
        (Seq.min dimensionValues, Seq.max dimensionValues))

/// Gets all conways located in a bounds
let rec GetAllConwaysInBounds (currentDimensionValues: int array) dimensionIndex (bounds: (int*int) array) = seq {
    match dimensionIndex = currentDimensionValues.Length with 
    | true -> 
        yield {Conway.Dimensions = currentDimensionValues}
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
    |> GetAllConwaysInBounds (Array.create targetCon.Dimensions.Length 0) 0
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
    |> Seq.where (fun con ->
        match Set.contains con conSet with
        | true -> // Con is active
            let surrounding = CountSurrounding con conSet
            match CountSurrounding con conSet with
            | 2 | 3 -> true
            | _ -> false
        | false -> // Con is inactive
            match CountSurrounding con conSet with
            | 3 -> true
            | _ -> false
        )
    |> Set.ofSeq

let rec SimulateCycles remainingCycles conSet  = 
    match remainingCycles with 
    | 0 -> conSet
    | _ -> SimulateCycles (remainingCycles - 1) (ProcessCycle conSet) 

let Part1 input = 
    input
    |> ScaleConwayDimensions 3
    |> SimulateCycles 6
    |> Set.count

let Part2 input = 
    input 
    |> ScaleConwayDimensions 4
    |> SimulateCycles 6 
    |> Set.count

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 359
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 2228
    0