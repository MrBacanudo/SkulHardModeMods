using UI.TestingTool;
using HarmonyLib;

namespace DevMenu;

[HarmonyPatch(typeof(Panel))]
public class DevMenuPatch
{
    [HarmonyPatch("canUse", MethodType.Getter)]
    static bool Prefix(ref Panel __instance, ref bool __result)
    {
        __result = true;
        return false;
    }
}
