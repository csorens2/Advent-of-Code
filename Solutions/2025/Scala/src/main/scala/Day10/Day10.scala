package Day10

import optimus.algebra.*

import scala.annotation.tailrec
import scala.collection.immutable.Queue
import scala.io.Source
import optimus.optimization.*
import optimus.optimization.enums.SolverLib
import optimus.optimization.model._

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
  def ProcessMachineBASE(toProcess: Machine): Int =
    implicit val model: MPModel = MPModel(SolverLib.oJSolver)

    val a = MPFloatVar("a", 0, INFINITE)
    val b = MPFloatVar("b", 0, INFINITE)
    val c = MPFloatVar("c", 0, INFINITE)
    val d = MPFloatVar("d", 0, INFINITE)
    val e = MPFloatVar("e", 0, INFINITE)
    val f = MPFloatVar("f", 0, INFINITE)

    minimize(a + b + c + d + e + f)

    add(3 := e + f)
    add(5 := b + f)
    add(4 := c + d + e)
    add(7 := a + b + d)


    start()
    println(s"objective: $objectiveValue")
    release()

    ???

  def ProcessMachine(toProcess: Machine): Int =
    implicit val model: MPModel = MPModel(SolverLib.oJSolver)
    def NumToVariable(num: Int): String =
      if num < 26 then
        ('a' + num).toChar.toString
      else
        ('a' + num % 26).toChar.toString + NumToVariable(num - 26)

    val buttonToFloatVar =
      toProcess
        .Buttons
        .indices
        .map(buttonIndex => (NumToVariable(buttonIndex), MPFloatVar(NumToVariable(buttonIndex), 0, INFINITE)))
        .toMap

    def FoldVoltageButtons(mapAcc: Map[Int, List[MPFloatVar]], nextButton: (String, Vector[Int])): Map[Int, List[MPFloatVar]] =
      val (nextButtonName, nextButtonVoltages) = nextButton
      val nextFloatVar = buttonToFloatVar(nextButtonName)
      nextButtonVoltages
        .foldLeft(mapAcc)((nextAcc, nextVoltage) =>
          nextAcc.get(nextVoltage) match
            case Some(prevButtons) => nextAcc + (nextVoltage -> (nextFloatVar :: prevButtons))
            case None => nextAcc + (nextVoltage -> List(nextFloatVar)))

    val voltageButtons: Map[Int, List[MPFloatVar]] =
      toProcess
        .Buttons
        .zipWithIndex
        .map((button, index) => (NumToVariable(index), button))
        .foldLeft(Map.empty)(FoldVoltageButtons)

    val emptyExpression: Expression = Zero
    val minimizeExpression =
      buttonToFloatVar
        .values
        .foldLeft(emptyExpression)((acc, next) => acc + next)

    minimize(minimizeExpression)

    for(voltageButton <- voltageButtons)
      val (voltage, variables) = voltageButton
      val addExpression =
        variables
          .foldLeft(emptyExpression)((acc, next) => acc + next)
      add(toProcess.Voltage(voltage) := addExpression)

    start()
    val toReturn = objectiveValue.toInt
    release()
    toReturn

  input
    .map(ProcessMachine)
    .sum
