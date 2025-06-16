
@echo off
start cmd /k "dotnet run --project MorpionServer"
timeout /t 2
start cmd /k "dotnet run --project MorpionClientGUI"
timeout /t 2
start cmd /k "dotnet run --project MorpionClientGUI"
