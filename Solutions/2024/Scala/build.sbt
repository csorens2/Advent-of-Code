ThisBuild / version := "0.1.0-SNAPSHOT"

ThisBuild / scalaVersion := "3.3.8"

lazy val root = (project in file("."))
  .settings(
    name := "Scala"
  )

libraryDependencies += "org.scalameta" %% "munit" % "1.3.4" % Test
libraryDependencies += "org.typelevel" %% "cats-collections-core" % "0.9.10"