@ECHO OFF

dotnet tool install dotnet-script --global --ignore-failed-sources  

dotnet script Program.csx

pause