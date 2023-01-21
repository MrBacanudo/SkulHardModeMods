using BepInEx;
using HarmonyLib;

namespace QuintessenceStart;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        Harmony.CreateAndPatchAll(typeof(QuintessencePatch));
        Logger.LogInfo($"Mod {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}
