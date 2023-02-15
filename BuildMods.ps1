# This script builds the dlls and puts them into the build folder
# Needs .NET installed, with the proper framework developer pack, etc.
dotnet.exe build  --configuration Release -o .\build\mods
