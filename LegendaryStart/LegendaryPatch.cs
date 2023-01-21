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
    static void Prefix(ref ManufacturingMachineInteractive __instance, ref List<GearReference> ____gearList, ref Gear.Type ____type){
        // Private property with get accessor, so we're using reflection instead of the triple underscore injection (?)
        PropertyInfo checkEnhanced = typeof(ManufacturingMachineInteractive).GetProperty("isEnhanced",  BindingFlags.NonPublic | BindingFlags.Instance);
        bool isEnhanced = (bool)checkEnhanced.GetValue(__instance);

        // We already have added the rares if we're in Level 8+, so we skip to prevent duplication here
        if (!(isEnhanced && ____type == Gear.Type.Weapon)){
            ____gearList.AddRange(Singleton<Service>.Instance.gearManager.GetGearListByRarity(____type, Rarity.Rare));
        }

        // Uniques and Legendaries
        ____gearList.AddRange(Singleton<Service>.Instance.gearManager.GetGearListByRarity(____type, Rarity.Unique));
        ____gearList.AddRange(Singleton<Service>.Instance.gearManager.GetGearListByRarity(____type, Rarity.Legendary));
    }
}
