module Day2

open System.IO
open System.Text.RegularExpressions

type Dimensions = {
    Length: int
    Width: int
    Height: int
}

let ParseInput filepath = 
    let mapFunc line =  
        let regexMatch = Regex("(\d+)x(\d+)x(\d+)").Match(line)
        {
            Dimensions.Length = int regexMatch.Groups[1].Value; 
            Width = int regexMatch.Groups[2].Value; 
            Height = int regexMatch.Groups[3].Value;
        }

    File.ReadLines(filepath)
    |> Seq.map mapFunc
    |> Seq.toList

let Part1 input =
    let mapFunc dimensions = 
        let sides = [
            (2 * dimensions.Length * dimensions.Width);
            (2 * dimensions.Width * dimensions.Height);
            (2 * dimensions.Height * dimensions.Length)
        ]
        let minSide = (List.min sides) / 2
        minSide + (List.sum sides)
    
    input
    |> List.map mapFunc
    |> List.sum

let Part2 input = 
    let calculateRibbon dimensions = 
        let sidesList = [
            dimensions.Height;
            dimensions.Width;
            dimensions.Length
        ]
        let smallestTwo = 
            sidesList
            |> List.sort 
            |> List.take 2
            |> List.toArray

        (smallestTwo[0] + smallestTwo[0] + smallestTwo[1] + smallestTwo[1]) + 
        (dimensions.Height * dimensions.Width * dimensions.Length)
    
    input
    |> List.map calculateRibbon
    |> List.sum