open System.IO
open System.Text.RegularExpressions

let ParseInput filepath = 
    let fieldRx = Regex(@"(\w\w\w):([#]?\w+)", RegexOptions.Compiled)
    let passports = 
        let rec groupedPassports (lines:seq<string>) (currentline:string) = seq {
            if lines |> Seq.isEmpty then
                yield currentline
            elif lines |> Seq.head = "" then
                yield currentline
                yield! groupedPassports (lines |> Seq.tail) ""
            else
                yield! groupedPassports (lines |> Seq.tail) (currentline + (lines |> Seq.head) + " " )
        }
        groupedPassports (File.ReadLines(filepath)) ""

    passports
    |> Seq.map(fun str -> 
        fieldRx.Matches(str) 
        |> Seq.map (fun (mat:Match) -> (mat.Groups[1].Value, mat.Groups[2].Value))
        |> Seq.fold (fun (acc:Map<string,string>) (nextKey, nextVal) -> acc.Add(nextKey, nextVal)) Map.empty)

let passportFields = [
    "byr";
    "iyr";
    "eyr";
    "hgt";
    "hcl";
    "ecl";
    "pid";
]

let Part1 (input:seq<Map<string,string>>) =             
    let rec passportValid (remainingFields:seq<string>) (passport:Map<string,string>) = 
        if Seq.isEmpty(remainingFields) then
            true
        elif not (passport.ContainsKey(remainingFields |> Seq.head)) then
            false
        else
            passportValid (Seq.tail(remainingFields)) passport
    
    input
    |> Seq.map (fun x -> passportValid passportFields x)
    |> Seq.fold (fun acc next -> if next then acc + 1 else acc) 0

let Part2 (input:seq<Map<string,string>>) =   
    let heightRegex = Regex(@"(\d+)(in|cm)", RegexOptions.Compiled)
    let hairColorRegex = Regex(@"#[0-9a-f]{6}$", RegexOptions.Compiled)
    let pidRegex = Regex(@"^\d{9}$", RegexOptions.Compiled)
    let eyecolorDict = Set.ofList [ 
        "amb"; 
        "blu";
        "brn";
        "gry";
        "grn";
        "hzl";
        "oth";
    ]

    let validateHeight (measurement:int) (unit:string) = 
        match unit with 
        | "cm" -> 150 <= measurement && measurement <= 193
        | "in" -> 59 <= measurement && measurement <= 76
        | _ -> failwith "Invalid Unit"

    let fieldValid fieldName fieldValue = 
        match fieldName with
        | "byr" -> 1920 <= int(fieldValue) && int(fieldValue) <= 2002
        | "iyr" -> 2010 <= int(fieldValue) && int(fieldValue) <= 2020
        | "eyr" -> 2020 <= int(fieldValue) && int(fieldValue) <= 2030
        | "hgt" -> heightRegex.Match(fieldValue).Success && (validateHeight (int(heightRegex.Match(fieldValue).Groups[1].Value)) heightRegex.Match(fieldValue).Groups[2].Value)
        | "hcl" -> hairColorRegex.Match(fieldValue).Success
        | "ecl" -> eyecolorDict.Contains(fieldValue)
        | "pid" -> pidRegex.Match(fieldValue).Success
        | _ -> failwith "Invalid field"

    let rec validatePassport remainingFields (passport:Map<string,string>) = 
        if remainingFields |> Seq.isEmpty then
            true
        else
            let nextField = remainingFields |> Seq.head
            let passportField = passport |> Map.tryFind nextField
            if passportField.IsNone then
                false
            elif not (fieldValid nextField passportField.Value) then
                false
            else
                validatePassport (remainingFields |> Seq.tail) passport
    input
    |> Seq.map (fun x -> validatePassport passportFields x)
    |> Seq.fold (fun acc next -> if next then acc + 1 else acc) 0

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 250
    let part2Result = Part2(input)
    printfn "Part 2 Result: %d" part2Result // 158
    0