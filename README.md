# SkulHardModeMods
Mods for the Dark Mirror of Skul: the Hero Slayer, PC/Steam Version

## Automatic Installation

(Soon)

## Manual Installation

1. Download the external dependencies
    * BepInEx (Windows x64): https://github.com/BepInEx/BepInEx/releases/download/v5.4.21/BepInEx_x64_5.4.21.0.zip
    * Unstripped Core Libraries: https://unity.bepinex.dev/corlibs/2020.3.34.zip
    * Unstripped Unity Libraries: https://unity.bepinex.dev/libraries/2020.3.34.zip
2. Extract all files into the Skul folder (default is `C:\Program Files (x86)\Steam\steamapps\common\Skul`)
    * BepInEx must be extracted to the game's root folder
    * The other two must be extracted inside the `2020.3.34` folder (the same folder!)
3. Edit the new `doorstop_config.ini` file inside the game folder:
    * Change the last line from `dllSearchPathOverride=` to `dllSearchPathOverride=2020.3.34`
4. Download the mods! https://github.com/MrBacanudo/SkulHardModeMods/releases
5. Put the mods you want into the `BepInEx/plugins` folder inside your game folder

Enjoy!