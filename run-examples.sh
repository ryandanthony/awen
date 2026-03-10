#!/bin/bash
# Script to run Awen with the example stories

dotnet build examples/ExampleUI.Stories/ExampleUI.Stories.csproj
dotnet run --project src/Awen -- --dir examples/ExampleUI.Stories/bin/Debug/net10.0/

