module Day10

open System.IO
open System.Text.RegularExpressions
open System.Collections.Immutable

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
    0