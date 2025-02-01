module Day5

open System.IO
open System.Text.RegularExpressions

let ParseInput filepath = 
    let parsePair line = 
        let lineMatch = (Regex("(\d+)\|(\d+)").Match line)
        (int lineMatch.Groups[1].Value, int lineMatch.Groups[2].Value)
    let parseList line =
        (Regex("(\d+)").Matches line)
        |> Seq.map (fun lineMatch -> int lineMatch.Groups[1].Value)
        |> Seq.toList

    let lines = 
        File.ReadLines(filepath)
        |> Seq.toList

    let pairs = 
        lines
        |> List.filter (fun line -> Regex("(\|)").IsMatch line)
        |> List.map parsePair

    let lists = 
        lines
        |> List.filter (fun line -> Regex("(,)").IsMatch line)
        |> List.map parseList

    (pairs, lists)
    
let ValidateUpdate orderingPairs toValidate = 
    let updateMap = 
        toValidate
        |> List.mapi (fun index value -> (value, index))
        |> Map.ofList

    let validatePair (left, right) = 
        if not (updateMap.ContainsKey left) || not (updateMap.ContainsKey right) then 
            true
        else
            Map.find left updateMap < Map.find right updateMap
    
    orderingPairs
    |> List.forall validatePair

let Part1 (input: ((int * int) list * int list list)) = 
    let (pairs, lists) = input
    
    lists
    |> List.filter (fun update -> ValidateUpdate pairs update)
    |> List.map (fun update -> update.Item (update.Length / 2))
    |> List.sum

let Part2 (input: ((int * int) list * int list list)) = 
    let (pairs, lists) = input

    let sortFunction int1 int2 = 
        let foundPair = 
            pairs
            |> List.tryFind (fun toFind -> toFind = (int1, int2) || toFind = (int2, int1))
        match foundPair with 
        | Some((left, right)) -> 
            if int1 = left then 
                -1
            else 
                1
        | None -> failwith "Unable to find pair"

    lists
    |> List.filter (fun update -> not (ValidateUpdate pairs update))
    |> List.map (fun update -> List.sortWith sortFunction update)
    |> List.map (fun update -> update.Item (update.Length / 2))
    |> List.sum
