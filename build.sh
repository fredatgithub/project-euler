#!/bin/sh
dotnet restore --verbosity minimal
dotnet build src/ProjectEuler
dotnet test tests/ProjectEuler.Tests
