using BepInEx;
using HarmonyLib;

namespace DeathTrap;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        Harmony.CreateAndPatchAll(typeof(DeathTrapPatch));
        Logger.LogInfo($"Mod {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}
