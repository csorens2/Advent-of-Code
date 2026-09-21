package Day10

import scala.annotation.tailrec
import scala.collection.immutable.Queue
import scala.io.Source

case class Machine(Lights: Vector[Boolean], Buttons: Vector[Vector[Int]], Voltage: Vector[Int])

def ParseFile(fileName: String) =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)
  val lines = fileSource.getLines()

  def ParseLine(line: String): Machine =
    val lineRegex = """\[(.*)\] (.*) \{(.*)\}""".r
    val lineMatch = lineRegex.findFirstMatchIn(line).get
    val lights =
      lineMatch
        .group(1)
        .map(matchChar => if matchChar == '.' then false else true)
        .toVector
    def mapButtonsAndVoltage(bvLine: String): Vector[Int] =
      val bvRegex = """(\d+)""".r
      bvRegex
        .findAllMatchIn(bvLine)
        .map(bvMatch => bvMatch.group(1).toInt)
        .toVector
    val buttons =
      lineMatch
        .group(2)
        .split(' ')
        .map(mapButtonsAndVoltage)
        .toVector
    val voltages = mapButtonsAndVoltage(lineMatch.group(3))
    Machine(lights, buttons, voltages)

  lines
    .map(ParseLine)
    .toVector

def Part1(input: Vector[Machine]): Int =
  def ProcessMachine(toProcess: Machine): Int =
    def PressButton(lightState: Vector[Boolean], button: Vector[Int]): Vector[Boolean] =
      button
        .foldLeft
          (lightState)
          ((lightAcc, nextLight) => lightAcc.updated(nextLight,!lightAcc(nextLight)))

    @tailrec
    def bfsToEnd(queue: Queue[(Vector[Boolean], Int, Set[Vector[Int]])]): Int =
      val ((queueLights, queueSteps, queueAvailableButtons), remainingQueue) = queue.dequeue
      if queueLights == toProcess.Lights then
        queueSteps
      else
        val nextQueue =
          queueAvailableButtons
            .foldLeft
            (remainingQueue)
            ((queueAcc, nextAvailableButton) =>
              queueAcc.enqueue(PressButton(queueLights, nextAvailableButton), queueSteps + 1, queueAvailableButtons - nextAvailableButton))
        bfsToEnd(nextQueue)
    bfsToEnd(Queue((Vector.fill(toProcess.Lights.length)(false), 0, toProcess.Buttons.toSet)))

  input
    .map(ProcessMachine)
    .sum

def Part2(input: Vector[Machine]): Int =
  def ProcessMachine(toProcess: Machine): Int =
    val finalVoltages = Vector.fill(toProcess.Voltage.length)(0)
    def DFSButtonPresses(remainingVoltages: Vector[Int], remainingVoltageChoices: Set[Int], numPresses: Int): Int =
      if remainingVoltageChoices.isEmpty then
          if remainingVoltages == finalVoltages then
            numPresses
          else
            throw Exception("Reached base case with non-final voltages")
      else
        def CountButtonsInfluencingGivenVoltage(voltageChoice: Int): Int =
          toProcess
            .Buttons
            .count(buttonVoltages => buttonVoltages.contains(voltageChoice))

        val chosenVoltage = remainingVoltageChoices.minBy(CountButtonsInfluencingGivenVoltage)

        if remainingVoltages(chosenVoltage) == 0 then
          DFSButtonPresses(remainingVoltages, remainingVoltageChoices - chosenVoltage, numPresses)
        else
          val voltageButtons =
            toProcess
              .Buttons
              .filter(buttonVoltages => buttonVoltages.contains(chosenVoltage))

          def getZeroedVoltages(remainingButtons: Set[Vector[Int]], currVoltages: Vector[Int]): List[Vector[Int]] =
            def pressButtonMulti(button: Vector[Int], voltage: Vector[Int], presses: Int): Vector[Int] =
              button
                .foldLeft(voltage)((voltageAcc, nextVoltage) =>
                  val remainingVoltage = voltageAcc(nextVoltage)
                  if remainingVoltage < presses then
                    throw Exception("Reducing voltage past 0")
                  else
                    voltageAcc.updated(nextVoltage, remainingVoltage - presses))

            def countMaxButtonPresses(button: Vector[Int], voltages: Vector[Int]): Int =
              button
                .map(buttonNum => voltages(buttonNum))
                .min

            val remainingChosenVoltage = currVoltages(chosenVoltage)
            if remainingButtons.size == 1 then
              val remainingButton = remainingButtons.head
              if countMaxButtonPresses(remainingButton, currVoltages) < remainingChosenVoltage then
                List.empty
              else
                List(pressButtonMulti(remainingButton, currVoltages, remainingChosenVoltage))
            else
              def processButton(buttonToProcess: Vector[Int]): List[Vector[Int]] =
                val nextMaxPresses = countMaxButtonPresses(buttonToProcess, currVoltages)
                  Range.inclusive(0, nextMaxPresses)
                    .toList
                    .map(presses => pressButtonMulti(buttonToProcess, currVoltages, presses))
                    .flatMap(nextVoltage => getZeroedVoltages(remainingButtons - buttonToProcess, nextVoltage))
              remainingButtons
                .flatMap(processButton)
                .toList

          val recursiveResult =
            getZeroedVoltages(voltageButtons.toSet, remainingVoltages)
              .distinct
              .map(zeroedVoltage => DFSButtonPresses(zeroedVoltage,remainingVoltageChoices - chosenVoltage, numPresses + remainingVoltages(chosenVoltage)))
          if recursiveResult.isEmpty then
            Int.MaxValue
          else
            recursiveResult.min

    DFSButtonPresses(toProcess.Voltage, toProcess.Voltage.indices.toSet, 0)

  val test =
    input
      .map(ProcessMachine)
  test
    .sum