//
//  main.swift
//  Day1
//
//  Created by Christopher Sorenson on 3/17/24.
//

import Foundation

@main
public struct Day1App
{
    public static func ProcessInput(fileName: String) -> [Int]
    {
        do 
        {
            let fileURL = Bundle.main.url(forResource: fileName, withExtension: nil)
            let content = try String(contentsOf: fileURL!, encoding: .utf8)
            let lines = content.split(separator: "\n").map{Int($0)}.map{$0!}
            
            return lines
        }
        catch 
        {
            print("ERROR: '\(error)' when attempting to open file '\(fileName)'")
            return []
        }
    }
    
    public static func CalculateFuelUsage(inputFuel: Int) -> Int
    {
        let nextFuel = Int(floor(Double(inputFuel) / 3.0)) - 2
        if nextFuel == 0 || nextFuel < 0
        {
            return 0
        }
        else
        {
            return nextFuel + CalculateFuelUsage(inputFuel: nextFuel)
        }
    }
    
    public static func Part1(input: [Int]) -> Int
    {
        return
            input
            .map {CalculateFuelUsage(inputFuel: $0)}
            .reduce(0, { $0 + $1 })
    }
    
    
    static func main() 
    {
        let input = ProcessInput(fileName: "Input.txt")
        let part1Result = Part1(input: input)
        print("Part1 Result: \(part1Result)") //3332538
        
        print("Hello, World!")
    }
    
    
}
