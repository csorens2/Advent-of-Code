module Day12

open System
open System.IO
open System.Text.RegularExpressions

let OperationalChar = '.'
let DamagedChar = '#'
let UnknownChar = '?'

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.map (fun line ->
        let springs = 
            Regex(@"([#.?]+)").Match(line).Value
        let conditionRecord = 
            Regex(@"(\d+)").Matches(line)
            |> Seq.map (fun recordMatch -> int recordMatch.Groups.[0].Value)
            |> Seq.toList
        (springs, conditionRecord))

let CharSeqToString charSeq = 
    String(Seq.toArray charSeq)

let rec GetSpringVariants springMap (springs: string) damagedGroups = 
    let remainingDamaged = List.sum damagedGroups
    match damagedGroups with 
    | [] ->
        let mapValue = 
            if not (springs.Contains(DamagedChar)) then
                1UL // Our base case representing a valid variant
            else 
                0UL
        Map.add (springs, remainingDamaged) mapValue springMap
    | nextGroup :: remainingGroups ->
        if Map.containsKey (springs, remainingDamaged) springMap then 
            springMap
        else
            // We don't care about leading operational springs when processing damaged chains.
            // But do make sure that the memoization map uses the base spring-string, and not the pruned one
            let prunedSprings = Seq.skipWhile (fun springChar -> springChar = OperationalChar) springs
            match Seq.tryHead prunedSprings with 
            | None -> Map.add (springs, remainingDamaged) 0UL springMap
            | Some nextSpring when nextSpring = DamagedChar -> 
                // Start a new damaged chain
                let rec processGroup (springSeq: seq<char>) remainingDamaged : seq<char> Option = 
                    match Seq.tryHead springSeq with 
                    | None when remainingDamaged = 0 -> Some(Seq.empty)
                    | Some nextSpring when remainingDamaged = 0 && (nextSpring = OperationalChar || nextSpring = UnknownChar) -> Some(Seq.tail springSeq)
                    | Some nextSpring when nextSpring = DamagedChar || nextSpring = UnknownChar -> processGroup (Seq.tail springSeq) (remainingDamaged - 1)
                    | _ -> None

                match processGroup prunedSprings nextGroup with 
                | None -> Map.add (springs, remainingDamaged) 0UL springMap
                | Some remainingSprings ->
                    let seqAsString = CharSeqToString remainingSprings
                    let subMap = GetSpringVariants springMap seqAsString remainingGroups

                    // When dealing with a non-unknown based chain, we just pass up whether the sub-chain was valid
                    Map.add (springs, remainingDamaged) subMap[(seqAsString, remainingDamaged - nextGroup)] subMap

            | Some nextSpring when nextSpring = UnknownChar -> 
                let damagedSpringVariant = CharSeqToString (Seq.append [DamagedChar] (Seq.tail prunedSprings))
                let subDamagedMap = GetSpringVariants springMap damagedSpringVariant damagedGroups
                let damagedTotal = subDamagedMap[(damagedSpringVariant, remainingDamaged)]

                let operationalSpringVariant = CharSeqToString (Seq.append [OperationalChar] (Seq.tail prunedSprings))
                let subOperationalMap = GetSpringVariants subDamagedMap operationalSpringVariant damagedGroups
                let operationalTotal = subOperationalMap[(operationalSpringVariant, remainingDamaged)]

                Map.add (springs, remainingDamaged) (damagedTotal + operationalTotal) subOperationalMap
            | _ -> failwith "This case should not be reached"
            

let UnfoldInput (input: seq<(String * int list)>) (foldCount: int) = 
    input
    |> Seq.map (fun (springs, groups) -> 
        let rec expandGroups seperationLambda numLeft toExpand = seq {
            if numLeft > 1 then
                yield! seperationLambda toExpand
                yield! expandGroups seperationLambda (numLeft - 1) toExpand
            elif numLeft = 1 then
                yield! toExpand
        }
        let expandedSprings = 
            expandGroups (fun toSeperate -> Seq.append toSeperate [UnknownChar]) foldCount springs
            |> CharSeqToString
        let expandedGroups = 
            expandGroups (id) foldCount groups
            |> Seq.toList
        let springVariantsMap = GetSpringVariants Map.empty expandedSprings expandedGroups
        springVariantsMap[(expandedSprings, List.sum expandedGroups)])
    |> Seq.sum

let Part1 input = 
    UnfoldInput input 1

let Part2 input = 
    UnfoldInput input 5

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 6981
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 4546215031609
    0