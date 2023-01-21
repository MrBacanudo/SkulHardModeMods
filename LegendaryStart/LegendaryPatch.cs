using System.Collections.Generic;
using System.Reflection;
using Characters.Gear;
using GameResources;
using Hardmode.Darktech;
using HarmonyLib;
using Services;
using Singletons;

[HarmonyPatch(typeof(ManufacturingMachineInteractive), "ActivateMachine")]
public class LegendaryPatch
{
    static void Prefix(ref ManufacturingMachineInteractive __instance){
        // Pretty!
        ref var self = ref __instance;

        // We already have added the rares if we're in Level 8+, so we skip to prevent duplication here
        if (!(self.isEnhanced && self._type == Gear.Type.Weapon)){
            self._gearList.AddRange(Singleton<Service>.Instance.gearManager.GetGearListByRarity(self._type, Rarity.Rare));
        }

        // Uniques and Legendaries
        self._gearList.AddRange(Singleton<Service>.Instance.gearManager.GetGearListByRarity(self._type, Rarity.Unique));
        self._gearList.AddRange(Singleton<Service>.Instance.gearManager.GetGearListByRarity(self._type, Rarity.Legendary));
    }
}
