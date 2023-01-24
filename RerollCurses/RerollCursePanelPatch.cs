using HarmonyLib;
using UI.Upgrades;
using UserInput;

namespace RerollCurses;

// Let's press buttons!

[HarmonyPatch(typeof(Panel), "Update")]
public class RerollCursePanelPatch
{
    static void Prefix(ref Panel __instance)
    {
        ref var self = ref __instance;
        if (self.focused && KeyMapper.Map.Quintessence.WasPressed)
        {
            Singletons.Singleton<Characters.Gear.Upgrades.UpgradeShop>.Instance.LoadCusredLineUp();
            self.UpdateAll();
        }
    }
}
