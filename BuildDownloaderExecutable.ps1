# This script builds the Downloader executable for Windows
# Requires PS2EXE installed (https://github.com/MScholtes/PS2EXE)
Invoke-PS2EXE .\Downloader.ps1 .\build\SkulModDownloader.exe -noConsole -noOutput -iconFile .\.resources\Download_Icon.ico
