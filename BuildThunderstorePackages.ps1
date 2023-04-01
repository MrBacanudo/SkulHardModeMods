$subfolders = Get-ChildItem -Directory

New-Item -ItemType "directory" -Path ".\build\packages\" -Force | Out-Null

foreach($subfolder in $subfolders) {
    if(Test-Path -Path $subfolder\manifest.json -PathType Leaf){
        dotnet build $subfolder --configuration Release
        $output_file = GET-ChildItem "$subfolder\bin\Release\*.dll" -File -Recurse
        $mod_name = (Get-Item $output_file).Basename

        $compress = @{
            Path = "$subfolder\manifest.json", "$subfolder\icon.png", "$subfolder\README.md", $output_file
            CompressionLevel = "Optimal"
            DestinationPath = ".\build\packages\$mod_name.zip"
        }
        Compress-Archive @compress -Force
    }
}

$modpack = @{
    Path = ".modpacks\HardModeSandbox\manifest.json", ".modpacks\HardModeSandbox\icon.png", ".modpacks\HardModeSandbox\README.md"
    CompressionLevel = "Optimal"
    DestinationPath = ".\build\packages\HardModeSandbox.zip"
}
Compress-Archive @modpack -Force

$memepack = @{
    Path = ".modpacks\MemePack\manifest.json", ".modpacks\MemePack\icon.png", ".modpacks\MemePack\README.md"
    CompressionLevel = "Optimal"
    DestinationPath = ".\build\packages\MemePack2023.zip"
}
Compress-Archive @memepack -Force
