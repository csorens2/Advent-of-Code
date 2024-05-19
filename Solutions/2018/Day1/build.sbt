ThisBuild / version := "0.1.0-SNAPSHOT"

ThisBuild / scalaVersion := "3.3.3"

lazy val copyRes = TaskKey[Unit]("copyRes")

lazy val root = (project in file("."))
  .settings(
    name := "Day1"
  )

libraryDependencies += "org.scalatest" %% "scalatest" % "3.2.18" % Test