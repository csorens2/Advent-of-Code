open System.IO
open System
open System.Text.RegularExpressions

type Password(min:int, max:int, letter:char, password:string) = 
    member this.Min = min
    member this.Max = max
    member this.Letter = letter
    member this.Password = password

let ParseInput filepath = 
    let rx = Regex(@"(\d+)[-](\d+) (\w+): (\w+)", RegexOptions.Compiled)
    File.ReadLines(filepath)      
    |> Seq.map(fun x -> rx.Match(x))
    |> Seq.map(fun x -> Password(int(x.Groups[1].Value), int(x.Groups[2].Value), char(x.Groups[3].Value), string(x.Groups[4].Value)))
    |> Seq.toList

let Part1 (passwords:seq<Password>) = 
    let validpassword (password:Password) = 
        let charCount = 
            password.Password
            |> Seq.where (fun x -> x = password.Letter)
            |> Seq.length
        password.Min <= charCount && charCount <= password.Max
    passwords
    |> Seq.where (fun x -> validpassword x)
    |> Seq.length

let Part1Optimized (passwords:seq<Password>) = 
    let validpassword (password:Password) = 
        let rec countletters remainingChars count = 
            if count > password.Max then
                false
            elif remainingChars |> Seq.isEmpty then
                if (password.Min <= count && count <= password.Max) then
                    true
                else
                    false
            else
                countletters (remainingChars |> Seq.tail) (count + 1)
        countletters (password.Password |> Seq.where (fun x -> x = password.Letter)) 0
    passwords
    |> Seq.where (fun x -> validpassword x)
    |> Seq.length

let Part2 (passwords:seq<Password>) = 
    let validpassword (password:Password) = 
        let minchar = password.Password[password.Min-1]
        let maxchar = password.Password[password.Max-1]
        (minchar <> maxchar) && ((minchar = password.Letter) || (maxchar = password.Letter))
    passwords
    |> Seq.where(fun x -> validpassword x)
    |> Seq.length

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1(input)
    printfn "Part 1 Result: %d" part1Result 
    let part12Result = Part1Optimized(input)
    printfn "Part 1 Take 2 Result: %d" part12Result 
    let part2Result = Part2(input)
    printfn "Part 2 Result: %d" part2Result
    0