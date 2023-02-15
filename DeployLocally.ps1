# This script builds the dlls directly into the plugins folder of the game

# Change this, if Skul is not installed on the default folder
$skul_folder = "${env:PROGRAMFILES(x86)}\Steam\steamapps\common\Skul\"

# Needs .NET installed, with the proper framework developer pack, etc.
dotnet.exe build  --configuration Release -o "$skul_folder\BepInEx\plugins"
