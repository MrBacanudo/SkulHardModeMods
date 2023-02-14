using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace NineItems;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static ConfigEntry<int> MaxItems;

    private void Awake()
    {
        MaxItems = Config.Bind<int>("General", "MaxItems", 9, "The number of available picks in the item machine.");
        Harmony.CreateAndPatchAll(typeof(NineItemPatch));
        Logger.LogInfo($"Mod {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}
