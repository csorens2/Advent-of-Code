package Day10

import optimus.algebra.*
import optimus.optimization.*
import optimus.optimization.enums.SolverLib
import optimus.optimization.model.MPIntVar
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
    implicit val model: MPModel = MPModel(SolverLib.oJSolver)
    def NumToVariable(num: Int): String =
      if num < 26 then
        ('a' + num).toChar.toString
      else
        ('a' + num % 26).toChar.toString + NumToVariable(num - 26)

    val largestVoltage = toProcess.Voltage.max

    val buttonToIntVar =
      toProcess
        .Buttons
        .indices
        .map(buttonIndex => (NumToVariable(buttonIndex), MPIntVar(NumToVariable(buttonIndex), 0 to largestVoltage)))
        .toMap

    def FoldVoltageButtons(mapAcc: Map[Int, List[MPIntVar]], nextButton: (String, Vector[Int])): Map[Int, List[MPIntVar]] =
      val (nextButtonName, nextButtonVoltages) = nextButton
      val nextIntVar = buttonToIntVar(nextButtonName)
      nextButtonVoltages
        .foldLeft(mapAcc)((nextAcc, nextVoltage) =>
          nextAcc.get(nextVoltage) match
            case Some(prevButtons) => nextAcc + (nextVoltage -> (nextIntVar :: prevButtons))
            case None => nextAcc + (nextVoltage -> List(nextIntVar)))

    val voltageButtons: Map[Int, List[MPIntVar]] =
      toProcess
        .Buttons
        .zipWithIndex
        .map((button, index) => (NumToVariable(index), button))
        .foldLeft(Map.empty)(FoldVoltageButtons)

    val emptyExpression: Expression = Zero
    val minimizeExpression =
      buttonToIntVar
        .values
        .foldLeft(emptyExpression)((acc, next) => acc + next)

    minimize(minimizeExpression)

    for(voltageButton <- voltageButtons)
      val (voltage, variables) = voltageButton
      val addExpression = variables.foldLeft(emptyExpression)((acc, next) => acc + next)
      add(toProcess.Voltage(voltage) := addExpression)

    start()
    val toReturn = math.round(objectiveValue).toInt
    release()
    toReturn

  input
    .map(ProcessMachine)
    .sum

