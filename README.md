# SkulHardModeMods
**Experimental** Mods for the Dark Mirror of Skul: the Hero Slayer, PC/Steam Version

The mod content is provided as-is and has no warranties. **Use them at your own risk!**

## Installing the mods through the Thunderstore

The Thunderstore provides us with a cool UI for automatic installation and updates for
[Mods](https://thunderstore.io/c/skul-the-hero-slayer/?ordering=most-downloaded) and [Mod packs](https://thunderstore.io/c/skul-the-hero-slayer/?ordering=most-downloaded&section=modpacks).
It is the recommended way of modding going forward.

1. Download the [latest r2modmanPlus](https://github.com/ebkr/r2modmanPlus/releases/latest)
2. Run it, find Skul: the Hero slayer
3. Install BepInEx with the installer
4. Install all the mods and/or modpacks you want!
5. Click "Run with mods"!

## Automatic Installation with PowerShell script (Old method)

1. Download the [Downloader script](Downloader.ps1)
   * Shortcut: Right click [this link](https://raw.githubusercontent.com/MrBacanudo/SkulHardModeMods/main/Downloader.ps1) and `Save Link As...` to download the script
2. Run with PowerShell
    * Right-clicking the file and selecting "Run with PowerShell" should work.
    * No PowerShell in your machine? [Learn how to Download Powershell here](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.3)
3. Select the game folder (you may need to minimize your windows to see the dialog)
    * Default is `C:\Program Files (x86)\Steam\steamapps\common\Skul` or `C:\Program Files\Steam\steamapps\common\Skul`
      * You can find this by right clicking Skul on Steam -> Properties -> Local Files -> Browse
4. If everything runs correctly, that should be it!

## Manual Installation (Advanced)

1. Download the external dependencies
    * BepInEx (Windows x64): https://github.com/BepInEx/BepInEx/releases/download/v5.4.21/BepInEx_x64_5.4.21.0.zip
    * Unstripped Core Libraries: https://unity.bepinex.dev/corlibs/2020.3.34.zip
    * Unstripped Unity Libraries: https://unity.bepinex.dev/libraries/2020.3.34.zip
2. Extract all files into the Skul folder
    * Default is `C:\Program Files (x86)\Steam\steamapps\common\Skul` or `C:\Program Files\Steam\steamapps\common\Skul`
      * You can find this by right clicking Skul on Steam -> Properties -> Local Files -> Browse
    * BepInEx must be extracted to the game's root folder (i.e. you must see `winhttp.dll` and `doorstop_config.ini` in that folder)
    * The other two must be extracted inside the `2020.3.34` folder (the same folder!)
3. Edit the new `doorstop_config.ini` file inside the game folder:
    * Change the last line from `dllSearchPathOverride=` to `dllSearchPathOverride=2020.3.34`
4. Download the mods! Latest one here: https://github.com/MrBacanudo/SkulHardModeMods/releases/latest
5. Put the mods you want into the `BepInEx/plugins` folder inside your game folder
    * You're allowed to mix or just use what you want!

Enjoy!
