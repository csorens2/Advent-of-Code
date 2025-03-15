//
//  Day1.swift
//  Day1
//
//  Created by Christopher Sorenson on 2/2/25.
//

import Foundation

public func ProcessInput(fileName: String) -> [Int]
{
    do
    {
        let fileURL = Bundle.main.url(forResource: fileName, withExtension: "txt")
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

public func CalculateNeededFuel(weight: Int) -> Int
{
    return Int(floor(Double(weight) / 3.0)) - 2
}

public func CalculateFuelUsage(nextWeight: Int) -> Int
{
    let nextFuel = CalculateNeededFuel(weight: nextWeight)
    if nextFuel <= 0
    {
        return 0
    }
    return nextFuel + CalculateFuelUsage(nextWeight: nextFuel)
}

public func Part1(input: [Int]) -> Int
{
    return
        input
        .map {CalculateNeededFuel(weight: $0)}
        .reduce(0, { $0 + $1 })
}

public func Part2(input: [Int]) -> Int
{
    return
        input
        .map {CalculateFuelUsage(nextWeight: $0)}
        .reduce(0, { $0 + $1 })
        
}
