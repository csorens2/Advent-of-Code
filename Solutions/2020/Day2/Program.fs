open System.IO
open System.Text.RegularExpressions

type Password = {
    Min: int
    Max: int
    Letter: char
    Password: string
}

let ParseInput filepath = 
    let rx = Regex(@"(\d+)[-](\d+) (\w+): (\w+)", RegexOptions.Compiled)
    File.ReadLines(filepath)      
    |> Seq.map(fun x -> rx.Match(x))
    |> Seq.map(fun x -> {
        Password.Min = int(x.Groups[1].Value); 
        Max = int(x.Groups[2].Value); 
        Letter = char(x.Groups[3].Value);
        Password = string(x.Groups[4].Value)}) 
    |> Seq.cache

let Part1 passwords = 
    let validpassword password = 
        let charCount = 
            password.Password
            |> Seq.where (fun x -> x = password.Letter)
            |> Seq.length
        password.Min <= charCount && charCount <= password.Max
    passwords
    |> Seq.where (fun x -> validpassword x)
    |> Seq.length

let Part1Optimized passwords = 
    let validpassword password = 
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

let Part2 passwords = 
    let validpassword password = 
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
    printfn "Part 1 Result: %d" part1Result // 548
    let part12Result = Part1Optimized(input)
    printfn "Part 1 Take 2 Result: %d" part12Result // 548
    let part2Result = Part2(input)
    printfn "Part 2 Result: %d" part2Result // 502
    0