package Day10

/*
def Part2(input: Vector[Machine]): Int =
  def ProcessMachine(toProcess: Machine): Int =

    val finalVoltages = Vector.fill(toProcess.Voltage.length)(0)
    val voltageToButtonsMap =
      toProcess.Voltage.indices
        .map(voltage => (voltage, toProcess.Buttons.filter(button => button.contains(voltage))))
        .toMap

    def DFSButtonPresses(remainingVoltages: Vector[Int], numPresses: Int): Int =
      val remainingVoltageChoices =
        remainingVoltages
          .zipWithIndex
          .filter((voltage, _) => voltage != 0)
          .map((_, index) => index)
      if remainingVoltageChoices.isEmpty then
          if remainingVoltages == finalVoltages then
            numPresses
          else
            throw Exception("Reached base case with non-final voltages")
      else

        val chosenVoltage = remainingVoltageChoices.toList.minBy(voltageChoice => voltageToButtonsMap(voltageChoice).size)

        if remainingVoltages(chosenVoltage) == 0 then
          DFSButtonPresses(remainingVoltages, numPresses)
        else
          val voltageButtons = voltageToButtonsMap(chosenVoltage)

          def getZeroedVoltages(remainingButtons: Set[Vector[Int]], currVoltages: Vector[Int]): Seq[Vector[Int]] =
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
              def processButton(buttonToProcess: Vector[Int]): Seq[Vector[Int]] =
                val nextMaxPresses = countMaxButtonPresses(buttonToProcess, currVoltages)
                  Range.inclusive(0, nextMaxPresses)
                    .map(presses => pressButtonMulti(buttonToProcess, currVoltages, presses))
                    .flatMap(nextVoltage => getZeroedVoltages(remainingButtons - buttonToProcess, nextVoltage))
              remainingButtons
                .toSeq
                .flatMap(processButton)

          val recursiveResult =
            getZeroedVoltages(voltageButtons.toSet, remainingVoltages)
              .map(zeroedVoltage => DFSButtonPresses(zeroedVoltage, numPresses + remainingVoltages(chosenVoltage)))
          if recursiveResult.isEmpty then
            Int.MaxValue
          else
            recursiveResult.min

    DFSButtonPresses(toProcess.Voltage, 0)

  input
    .map(ProcessMachine)
    .sum

 */
