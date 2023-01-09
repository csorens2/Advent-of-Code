open System
open System.IO
open Microsoft.FSharp.Collections

let ParseInput filepath = 
    let rec buildGroups (currentGroup:seq<string>) (remainingLines:seq<string>) = seq {
        if Seq.isEmpty(remainingLines) then
            yield currentGroup
        elif Seq.head(remainingLines) = String.Empty then
            yield currentGroup
            yield! buildGroups Seq.empty (Seq.tail(remainingLines))
        else
            yield! buildGroups (Seq.append currentGroup ([Seq.head(remainingLines)])) (Seq.tail(remainingLines))
    }
    buildGroups Seq.empty (File.ReadLines(filepath))

let Part1 (groups:seq<seq<string>>) =     
    let getGroupOverlap (group:seq<string>) = 
        group
        |> Seq.map (fun x -> x |> Seq.fold (fun (acc:Set<char>) next -> acc.Add(next)) Set.empty)
        |> Seq.fold (fun (acc:Set<char>) next -> Set.union acc next) Set.empty
        |> Seq.length

    groups
    |> Seq.map (getGroupOverlap)
    |> Seq.sum

let Part2 (groups:seq<seq<string>>) = 
    let processGroup (group:seq<string>) = 
        group
        |> Seq.map (fun x -> x |> Seq.fold (fun (acc:Set<char>) next -> acc.Add(next)) Set.empty)
        |> Seq.reduce (fun acc next -> Set.intersect acc next)
        |> Seq.length

    groups
    |> Seq.map (processGroup)
    |> Seq.sum

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 6259
    let part2Result = Part2(input)
    printfn "Part 2 Result: %d" part2Result
    0