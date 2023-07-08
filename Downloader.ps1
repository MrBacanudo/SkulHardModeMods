# Stop on any error
$ErrorActionPreference = "Stop"

# Select the Skul Folder
Add-Type -AssemblyName System.Windows.Forms
$Browser = New-Object System.Windows.Forms.FolderBrowserDialog

$default_folder = "${env:PROGRAMFILES(x86)}\Steam\steamapps\common\Skul"
$default_folder2 = "${env:PROGRAMFILES}\Steam\steamapps\common\Skul"

if(Test-Path -Path $default_folder\Skul.exe -PathType Leaf){
    $folder = $default_folder
} elseif(Test-Path -Path $default_folder2\Skul.exe -PathType Leaf){
    $folder = $default_folder2
} elseif($Browser.ShowDialog() -eq "OK") {
    $folder = $Browser.SelectedPath
} else {
    [System.Windows.Forms.MessageBox]::Show("You need to select a Folder. Try again.", "Error")
    Exit
}

if(-not(Test-Path -Path $folder\Skul.exe -PathType Leaf)){
    [System.Windows.Forms.MessageBox]::Show("The folder does not have a Skul.exe file. Did you pick the correct one?", "Error")
    Exit
}

# Download Files
try {
    $download_folder = Join-Path $folder ".downloads"
    $unity_version = "2020.3.34"
    New-Item -ItemType "directory" -Path $download_folder -Force | Out-Null
    Invoke-WebRequest -URI "https://github.com/BepInEx/BepInEx/releases/download/v5.4.21/BepInEx_x64_5.4.21.0.zip" -OutFile $download_folder/BepInEx.zip
    Invoke-WebRequest -URI "https://unity.bepinex.dev/corlibs/$unity_version.zip" -OutFile $download_folder/corlib.zip
    Invoke-WebRequest -URI "https://unity.bepinex.dev/libraries/$unity_version.zip" -OutFile $download_folder/unity.zip
    Invoke-WebRequest -URI "https://github.com/MrBacanudo/SkulHardModeMods/releases/download/HardModeModPack-v0.0.6/AllMods.zip" -OutFile $download_folder/modpack.zip
} catch {
    [System.Windows.Forms.MessageBox]::Show("An error occured while trying to download files.", "Error")
    Exit
}

# Unzip all files
try {
    Expand-Archive -Path $download_folder/BepInEx.zip -DestinationPath $folder -Force
    Expand-Archive -Path $download_folder/corlib.zip -DestinationPath $folder/$unity_version -Force
    Expand-Archive -Path $download_folder/unity.zip -DestinationPath $folder/$unity_version -Force
    Expand-Archive -Path $download_folder/modpack.zip -DestinationPath $folder/BepInEx/plugins -Force
} catch {
    [System.Windows.Forms.MessageBox]::Show("Could not extract the files.", "Error")
    Exit
}

# Configure Doorstop
try {
    $doorstop_content = Get-Content $folder\doorstop_config.ini
    $doorstop_content = $doorstop_content[0..($doorstop_content.length-2)]
    $doorstop_content = $doorstop_content + "dllSearchPathOverride=$unity_version"
    $doorstop_content | Out-File $folder\doorstop_config.ini -Force
} catch {
    [System.Windows.Forms.MessageBox]::Show("Could not configure doorstop.", "Error")
    Exit
}

# Show message of conclusion, if done
[System.Windows.Forms.MessageBox]::Show("Installer concluded Successfully! Enjoy the mods!", "Success!")
