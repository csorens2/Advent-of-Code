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
    
    public static func CalculateNeededFuel(weight: Int) -> Int
    {
        return Int(floor(Double(weight) / 3.0)) - 2
    }
    
    public static func CalculateFuelUsage(nextWeight: Int) -> Int
    {
        let nextFuel = CalculateNeededFuel(weight: nextWeight)
        if nextFuel <= 0
        {
            return 0
        }
        return nextFuel + CalculateFuelUsage(nextWeight: nextFuel)
    }
    
    public static func Part1(input: [Int]) -> Int
    {
        return
            input
            .map {CalculateNeededFuel(weight: $0)}
            .reduce(0, { $0 + $1 })
    }
    
    public static func Part2(input: [Int]) -> Int
    {
        return
            input
            .map {CalculateFuelUsage(nextWeight: $0)}
            .reduce(0, { $0 + $1 })
            
    }
    
    
    static func main() 
    {
        let input = ProcessInput(fileName: "Input.txt")
        let part1Result = Part1(input: input)
        let part2Result = Part2(input: input)
        print("Part1 Result: \(part1Result)") //3332538
        print("Part2 Result: \(part2Result)") //4995942
        print("Hello, World!")
    }
    
    
}
