using Characters;
using Characters.Gear.Weapons;
using Hardmode.Darktech;
using HarmonyLib;
using Level;
using Services;
using Singletons;

namespace AwakeOnCastle;

[HarmonyPatch(typeof(DimensionMirrorObservationInstrument))]
public class AwakePatch
{
    static bool canAwake(DimensionMirrorObservationInstrument self)
    {
        Character player = Singleton<Service>.Instance.levelManager.player;
        Weapon current = player.playerComponents.inventory.weapon.current;

        return !current.name.Equals(self._skulName) && !current.name.Equals(self._heroSkulName) && current.rarity != Rarity.Legendary;
    }

    [HarmonyPostfix]
    [HarmonyPatch("Update")]
    static void AllowInteractingWithMirror(ref DimensionMirrorObservationInstrument __instance)
    {
        ref var self = ref __instance;

        if (self._character == null || !canAwake(self))
        {
            return;
        }

        self._uiObject.SetActive(true);
    }

    [HarmonyPostfix]
    [HarmonyPatch("InteractWith")]
    static void AwakeByInteractingWithMirror(ref DimensionMirrorObservationInstrument __instance)
    {
        if (!canAwake(__instance))
        {
            return;
        }

        LevelManager levelManager = Singleton<Service>.Instance.levelManager;
        levelManager.player.playerComponents.inventory.weapon.UpgradeCurrentWeapon();
    }

}
