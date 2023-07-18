open System
open System.IO
open System.Text.RegularExpressions

type Rule = 
    | Character of value: string
    | SubRules of value: int

let ParseInput filepath = 
    let rec parseRule currRule remainingString = seq {
        match String.IsNullOrEmpty remainingString with
        | true -> yield List.rev currRule
        | false -> 
            let numRuleRegex = Regex(@"^ (\d+)")
            let charRuleRegex = Regex("^ \"(\\w+)\"")
            let newRuleRegex = Regex(@"^ \|")
            let ruleList = [numRuleRegex; charRuleRegex; newRuleRegex]
            let correctRule = (List.find (fun (rule:Regex) -> rule.IsMatch remainingString) ruleList)
            let matchValue = (correctRule.Match remainingString)
            let nextRemaining = remainingString.Substring matchValue.Groups[0].Length
            let ruleValue = matchValue.Groups[1].Value
            // Regexs does not implement IEquality, so they can't be matched directly. We have to go back to comparing the original string
            if numRuleRegex.IsMatch remainingString then
                yield! parseRule (SubRules(int ruleValue) :: currRule) nextRemaining
            elif charRuleRegex.IsMatch remainingString then
                yield! parseRule (Character(ruleValue) :: currRule) nextRemaining
            elif newRuleRegex.IsMatch remainingString then
                yield List.rev currRule
                yield! parseRule List.empty nextRemaining
            else
                raise (Exception "No match found.")
    }
    let ruleNumRegex = (Regex(@"(.+):"))
    let lines = Seq.toList (File.ReadLines(filepath))
    let rules = List.takeWhile (fun line -> not (String.IsNullOrWhiteSpace line)) lines
    let messages = List.skipWhile (fun (line:string) -> (ruleNumRegex.IsMatch line) || (String.IsNullOrWhiteSpace line)) lines
    let parsedRules = 
        rules
        |> Seq.fold (fun mapAcc (nextLine: string) ->
            let ruleNumRegexMatch = ruleNumRegex.Match nextLine
            let remainingString = nextLine.Substring ruleNumRegexMatch.Groups[0].Value.Length
            Map.add (int ruleNumRegexMatch.Groups[1].Value) (Seq.toList (parseRule List.empty remainingString)) mapAcc
            ) Map.empty
    (parsedRules, messages)
   
let Part1 (rules, messages) = 
    1

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 
    //let part2Result = Part2 input
    //printfn "Part 2 Result: %d" part2Result // 
    0