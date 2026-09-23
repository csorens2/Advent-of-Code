module Day10

open System.IO
open System.Text.RegularExpressions
open System.Collections.Immutable
open Flips
open Flips.Types

type Machine = {
    Lights: bool array
    Buttons: int array array
    Voltage: int array
}

let ParseInput filepath = 
    
    let parseLine line = 
        let lineMatch = Regex(@"\[(.*)\] (.*) \{(.*)\}").Match(line)
        let lights = 
            lineMatch.Groups[1].Value
            |> Seq.map (fun lightChar -> if lightChar = '.' then false else true)
            |> Seq.toArray
        let mapButtonsAndVoltages bvLine = 
            Regex(@"(\d+)").Matches(bvLine)
            |> Seq.map (fun bvMatch -> int bvMatch.Groups[1].Value)
            |> Seq.toArray
        let buttons = 
            lineMatch.Groups[2].Value.Split(' ')
            |> Seq.map (mapButtonsAndVoltages)
            |> Seq.toArray
        let voltage = mapButtonsAndVoltages lineMatch.Groups[3].Value
        {Machine.Lights = lights; Buttons = buttons; Voltage = voltage}

    File.ReadLines(filepath)
    |> Seq.map parseLine

let Part1 input = 
    
    let processMachine toProcess = 
        let pressButton lightState button = 
            Array.fold 
                (fun lightAcc nextLight -> Array.updateAt nextLight (not lightAcc[nextLight]) lightAcc) 
                lightState 
                button
        
        let rec bfsToEnd (queue: ImmutableQueue<(bool array * int * Set<int array>)>) = 
            let (queueLights, queueSteps, queueAvailableButtons) = queue.Peek()
            let remainingQueue = queue.Dequeue()
            if queueLights = toProcess.Lights then 
                queueSteps
            else
                let availableButtonsFolder (queueAcc: ImmutableQueue<(bool array * int * Set<int array>)>) nextAvailableButton = 
                    queueAcc.Enqueue(
                    (
                        pressButton queueLights nextAvailableButton,
                        queueSteps + 1,
                        Set.remove nextAvailableButton queueAvailableButtons
                    ))

                bfsToEnd (Set.fold availableButtonsFolder remainingQueue queueAvailableButtons)

        bfsToEnd 
            (ImmutableQueue.Empty.Enqueue(
                (
                    Array.init (Array.length toProcess.Lights) (fun _ -> false),
                    0,
                    Set.ofArray toProcess.Buttons
                )
            ))

    Seq.sumBy processMachine input

let Part2 input = 
    
    let processMachine toProcess = 
        let rec numToVariable num = 
            if num < 26 then 
                string (char ((int 'a') + num))
            else
                (string (char ((int 'a') + num))) + numToVariable (num - 26)
        
        let largestVoltage = Array.max toProcess.Voltage

        let buttonToDecision = 
            [0..(Array.length toProcess.Buttons) - 1]
            |> List.map (
                fun buttonIndex -> 
                    (
                        numToVariable buttonIndex, 
                        Decision.createContinuous (numToVariable buttonIndex) 0.0 infinity)
                    )
            |> Map.ofList
        
        let foldVoltageButtons mapAcc (nextButtonName, nextButtonVoltages) =
            let nextDecision = buttonToDecision[nextButtonName]
            nextButtonVoltages
            |> Array.fold 
                (fun nextAcc nextVoltage -> 
                    match Map.tryFind nextVoltage nextAcc with 
                    | Some(prevButtons) -> Map.add nextVoltage (nextDecision :: prevButtons) nextAcc
                    | None -> Map.add nextVoltage [nextDecision] nextAcc)
                mapAcc

        let voltageButtons = 
            toProcess.Buttons
            |> Array.indexed
            |> Array.map (fun (index,button) -> (numToVariable index, button))
            |> Array.fold foldVoltageButtons Map.empty

        let minimizeExpression = 
            buttonToDecision
            |> Map.values
            |> Seq.fold (fun acc next -> acc + next) LinearExpression.Empty 

        let objective = Objective.create "Minimize Button Pushes" Minimize minimizeExpression

        let foldModel (acc: Model.Model) (voltage, decisions) =
            let addConstraint = List.fold (fun acc next -> acc + next) LinearExpression.Empty decisions
            Model.addConstraint (Constraint.create  ) acc
            

        let model = Model.create objective
        
        0
    
    
    Seq.sumBy processMachine input