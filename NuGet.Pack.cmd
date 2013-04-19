@echo off

set NUGET="src/.nuget/nuget.exe"

if not exist output ( mkdir output )

%NUGET% Pack "src/PerformanceCounters/PerformanceCounters.nuspec" -OutputDirectory output
