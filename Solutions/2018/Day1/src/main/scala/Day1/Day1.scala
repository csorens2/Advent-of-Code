package Day1

import scala.io.Source

def ReadFile(input: String): Unit =
  val resource = Source.getClass.getResource(input)
  //val resource: URL = getClass.getResource("<my_resource_name>")
  //val fileSource = Source.fromFile(input)
  val test = 1
