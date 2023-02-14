open System
open System.IO
open System.Text.RegularExpressions

type Program(mask: string, values: seq<int64*int64>) =
    member this.BitMask = mask
    member this.Values = values

let ParseInput filepath = 
    let mask_rx = Regex(@"mask = (\w+)", RegexOptions.Compiled)
    let address_rx = Regex(@"mem\[(\d+)\] = (\d+)", RegexOptions.Compiled)
    let rec get_programs (lines: seq<String>) = seq {
        if not (Seq.isEmpty lines) then
            let bitMask = mask_rx.Match(Seq.head lines).Groups[1].Value
            let memAddressesLines = Seq.tail lines
            let memAddresses = 
                memAddressesLines
                |> Seq.takeWhile(fun x -> address_rx.IsMatch(x))
                |> Seq.map (fun x -> address_rx.Match(x))
                |> Seq.map (fun x -> (int64 x.Groups[1].Value, int64 x.Groups[2].Value))
            yield Program(bitMask, memAddresses)
            let nextLines = 
                memAddressesLines
                |> Seq.skipWhile(fun x -> not (mask_rx.IsMatch(x)))
            yield! get_programs nextLines
    }
    get_programs (File.ReadLines(filepath))

let getMaskedValue (mask:seq<char>) (intValue:int64) = 
    let rec rec_getMaskedValue (currentVal:int64) (maskBits:seq<char>) (bitWeight:int64) = 
        if Seq.isEmpty maskBits then
            currentVal
        else
            let possibleBitPosVal = int64 (Math.Pow(2, int bitWeight))
            let nextBitVal = 
                match Seq.head maskBits with
                | '1' -> possibleBitPosVal
                | '0' -> 0
                | 'X' -> ((intValue >>> (int bitWeight)) &&& 1) * possibleBitPosVal
                | _ -> failwith "Unexpected mask value"
            rec_getMaskedValue (currentVal + nextBitVal) (Seq.tail maskBits) (bitWeight + 1L)              
    rec_getMaskedValue 0 (Seq.rev mask) 0

let Part1 (input:seq<Program>) = 
    input
    |> Seq.fold (fun (acc: Map<int64,int64>) nextProg -> 
        nextProg.Values
        |> Seq.fold(fun (accInner: Map<int64,int64>) (address,value) -> 
            Map.add address (getMaskedValue nextProg.BitMask value) accInner) acc
    ) Map.empty
    |> Map.values
    |> Seq.reduce (fun acc next -> acc + next)

let getAddresses mask (intValue:int64) = 
    let rec rec_getAddresses (addresses:seq<int64>) (maskBits:seq<char>) (bitWeight:int) = 
        if Seq.isEmpty maskBits then
            addresses
        else
            let possibleBitPosVal = int64 (Math.Pow(2, int bitWeight))
            if (Seq.head maskBits) = 'X' then
                let rec doubledAddresses (remainingAddresses:seq<int64>) = seq {
                    if not (Seq.isEmpty remainingAddresses) then
                        yield Seq.head remainingAddresses
                        yield (Seq.head remainingAddresses) + possibleBitPosVal
                        yield! doubledAddresses (Seq.tail remainingAddresses)
                }
                rec_getAddresses ((doubledAddresses addresses) |> Seq.toList) (Seq.tail maskBits) (bitWeight + 1)
            else
                let bitValue = 
                    (intValue >>> bitWeight) &&& 1
                let updatedAddresses = 
                    match Seq.head maskBits with
                    | '0' -> addresses |> Seq.map (fun x -> x + (bitValue * possibleBitPosVal))
                    | '1' -> addresses |> Seq.map (fun x -> x + possibleBitPosVal)
                    | _ -> failwith "Unexpected Address Value"
                rec_getAddresses (updatedAddresses) (Seq.tail maskBits) (bitWeight + 1)
    rec_getAddresses ([0]) (Seq.rev mask) 0

let Part2 (input:seq<Program>) = 
    input
    |> Seq.fold (fun (acc: Map<int64,int64>) nextProg -> 
        nextProg.Values
        |> Seq.fold (fun (accInner: Map<int64,int64>) (nextBaseAdd, nextBaseVal) -> 
            getAddresses nextProg.BitMask nextBaseAdd
            |> Seq.fold (fun (accInnerInner: Map<int64,int64>) address ->
                Map.add address nextBaseVal accInnerInner
            ) accInner
        ) acc
    ) Map.empty
    |> Map.values
    |> Seq.reduce (fun acc next -> acc + next)

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 15018100062885
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 5724245857696
    0