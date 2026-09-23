ThisBuild / version := "0.1.0-SNAPSHOT"

ThisBuild / scalaVersion := "3.3.8"

lazy val root = (project in file("."))
  .settings(
    name := "2025"
  )

libraryDependencies += "org.scalameta" %% "munit" % "1.3.6" % Test
libraryDependencies ++= Seq(
  "com.github.vagmcs" %% "optimus" % "3.4.5",
  "com.github.vagmcs" %% "optimus-solver-oj" % "3.4.5"
)