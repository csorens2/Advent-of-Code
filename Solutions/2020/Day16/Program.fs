open System
open System.IO
open System.Text.RegularExpressions

type InputData = {
    TicketFields: Map<string,(int*int) list>
    YourTicket: int array
    NearbyTickets: int array list
}

let ParseInput filepath = 
    let ticketFieldRegex = Regex(@"((\d+)-(\d+))")
    let yourTicketLabel = "your ticket:"
    let nearbyTicketsLabel = "nearby tickets:"
    let processTickets lines = 
        let ticketsRegex = Regex(@"(\d+)")
        lines
        |> List.map (fun line ->
            (ticketsRegex.Matches line)
            |> Seq.cast<Match>
            |> Array.ofSeq
            |> Array.map (fun matchValue -> int matchValue.Groups[1].Value))
    let nextSection lines = 
        let skippedLines = List.skipWhile (fun line -> not (String.IsNullOrWhiteSpace line)) lines
        match skippedLines with
        | [] -> []
        | _ -> skippedLines |> List.skip 1
    let rec processLines inputData remainingLines  =
        match remainingLines with 
        | [] -> inputData
        | nextLine :: _ -> 
            match (nextLine:string) with
            | _ when nextLine.Contains yourTicketLabel ->
                let yourTicket = processTickets (remainingLines |> List.skip 1 |> List.take 1)
                processLines
                    {inputData with YourTicket = yourTicket[0]}
                    (nextSection remainingLines)
            | _ when nextLine.Contains nearbyTicketsLabel ->
                let nearbyTickets = processTickets (remainingLines |> List.skip 1 |> List.takeWhile (fun line -> not (String.IsNullOrWhiteSpace line)))
                processLines
                    {inputData with NearbyTickets = nearbyTickets}
                    (nextSection remainingLines)
            | _ -> // Ticket fields
                let nameFieldRegex = Regex(@"(.+):")
                let ticketFields = 
                    remainingLines
                    |> List.takeWhile (fun line -> ticketFieldRegex.IsMatch line)
                    |> List.fold (fun accMap nextLine -> 
                        let name = (nameFieldRegex.Match nextLine).Groups[1].Value
                        let values = 
                            (ticketFieldRegex.Matches nextLine)
                            |> Seq.cast<Match>
                            |> List.ofSeq
                            |> List.map (fun matchValue -> (int matchValue.Groups[2].Value, int matchValue.Groups[3].Value))
                        Map.add name values accMap) Map.empty
                processLines
                    {inputData with TicketFields = ticketFields}
                    (nextSection remainingLines)  
    processLines 
        {InputData.TicketFields = Map.empty; YourTicket = Array.empty; NearbyTickets = List.empty}
        (File.ReadLines(filepath) |> Seq.toList)

let IsValidTicketValue value fields = 
    fields
    |> Map.values
    |> Seq.tryFind (fun ranges ->
        ranges
        |> List.tryFind (fun (left,right) -> (left <= value) && (value <= right))
        |> Option.isSome)
    |> Option.isSome

let Part1 input = 
    input.NearbyTickets
    |> List.fold (fun ticketsAcc nextTicket -> 
        ticketsAcc + 
        (nextTicket
        |> Array.map (fun value -> if IsValidTicketValue value input.TicketFields then 0 else value)
        |> Array.sum)) 0

let Part2 (input: InputData) =   
    let rec GetTicketFields (remainingFields: Map<string,(int*int) list>) (tickets: int array list) (ticketIndex: int) = seq {
        match remainingFields.Count with 
        | 1 -> 
            let (finalField, _) = 
                remainingFields 
                |> Map.toList 
                |> List.head
            yield finalField
        | _ ->
            let ticketValues = 
                tickets
                |> List.map (fun ticket -> ticket[ticketIndex])
            let (targetFieldName, _) = 
                remainingFields
                |> Map.toList
                |> List.find (fun (name, fieldList) -> 
                    fieldList
                    |> List.forall (fun (leftBound, rightBound) -> 
                        (List.tryFind (fun value -> leftBound <= value && value <= rightBound) ticketValues).IsSome
                    )
                )
            yield targetFieldName
            yield! GetTicketFields (Map.remove targetFieldName remainingFields) tickets (ticketIndex + 1)
    }
    let departureTag = "departure"
    let prunedTickets = 
        input.NearbyTickets
        |> List.where (fun ticket -> 
            (ticket 
            |> Array.tryFind (fun value -> not (IsValidTicketValue value input.TicketFields)))
            |> Option.isNone)
    input.YourTicket
    |> Seq.zip (GetTicketFields input.TicketFields prunedTickets 0)
    |> Seq.where (fun (name, _) -> name.Contains(departureTag))
    |> Seq.fold (fun acc (_, next) -> acc * (uint64 next)) 1UL
        

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 23036
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 
    0