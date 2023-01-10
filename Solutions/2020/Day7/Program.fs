open System.IO
open System.Text.RegularExpressions

type Bag(adjective:string, color:string, subBags:seq<(string*string)*int>) = 
    member this.Adjective = adjective
    member this.Color = color
    member this.SubBags = subBags
    member this.Name with get() = (this.Adjective, this.Color)
    member this.SubBagNames with get() = this.SubBags |> Seq.map (fun (x,_) -> x)

let ParseInput filepath = 
    let lineRegex = Regex(@"((\w+) (\w+) bags contain)|([,]?(\d+) (\w+) (\w+) bag[s]?)", RegexOptions.Compiled)
    let processLine (line:string) = 
        let lineMatches = lineRegex.Matches(line)
        let bagAdj = (Seq.head(lineMatches)).Groups[2].Value
        let bagColor = (Seq.head(lineMatches)).Groups[3].Value
        if lineMatches.Count = 1 then
            Bag(bagAdj, bagColor, Seq.empty)
        else
            let subBags = 
                lineMatches
                |> Seq.skip 1
                |> Seq.map (fun x -> ((x.Groups[6].Value, x.Groups[7].Value), int(x.Groups[5].Value)))
            Bag(bagAdj, bagColor, subBags)
    
    File.ReadLines(filepath)
    |> Seq.map (fun x -> processLine x)

let Part1 (bags:seq<Bag>) = 
    let targetBag = ("shiny", "gold")
    let bagMap = 
        bags
        |> Seq.fold (fun (acc:Map<string*string, Bag>) next -> acc.Add(next.Name, next)) Map.empty
    let baseGoldMap = 
        bags
        |> Seq.fold (fun (acc:Map<string*string, Option<bool>>) next -> acc.Add(next.Name, None)) Map.empty
    let rec processBag (goldMap:Map<string*string, Option<bool>>) (bag:Bag) = 
        let rec processSubBags (goldMap:Map<string*string, Option<bool>>) (bagName:string*string) (remainingSubBags:seq<string*string>) = 
            if Seq.isEmpty(remainingSubBags) then
                goldMap.Add(bagName, Some(false))
            else
                let nextSubBag = Seq.head(remainingSubBags)
                let nextMap = 
                    if goldMap[nextSubBag].IsSome then
                        goldMap
                    else
                        processBag goldMap bagMap[nextSubBag]
                if nextMap[nextSubBag].Value = true then
                    nextMap.Add(bagName, Some(true))
                else
                    processSubBags nextMap bagName (Seq.tail(remainingSubBags))           
        let bagName = bag.Name
        if goldMap[bagName].IsSome then
            goldMap
        elif bagName = targetBag then
            goldMap.Add(bagName, Some(true))
        elif Seq.isEmpty(bag.SubBags) then
            goldMap.Add(bagName, Some(false))
        else
            processSubBags goldMap bagName bag.SubBagNames

    let goldCount = 
        bags
        |> Seq.fold (fun acc next -> processBag acc next ) baseGoldMap
        |> Map.values
        |> Seq.map (fun x -> x.Value)
        |> Seq.filter(fun x -> x)
        |> Seq.length

    goldCount - 1

let Part2 (bags:seq<Bag>) = 
    let starterBag = ("shiny", "gold")
    let bagMap = 
        bags
        |> Seq.fold (fun (acc:Map<string*string, Bag>) next -> acc.Add(next.Name, next)) Map.empty
    let rec ProcessBag (bagName:string*string) = 
        let nextBag = bagMap[bagName]
        if Seq.isEmpty(nextBag.SubBags) then
            1
        else
            let seed = if bagName = starterBag then 0 else 1
            nextBag.SubBags
            |> Seq.fold (fun acc (nextBag, bagCount) -> acc + (bagCount * ProcessBag nextBag)) seed
    
    ProcessBag ("shiny", "gold")

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 121
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 3805
    0
