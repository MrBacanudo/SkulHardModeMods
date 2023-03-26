using System.Collections.Generic;
using System.Reflection;
using Characters.Gear;
using GameResources;
using Hardmode.Darktech;
using HarmonyLib;
using Services;
using Singletons;
using System.Linq;
using Characters.Gear.Synergy.Inscriptions;

namespace HiddenItemsAndSkullsStart;

[HarmonyPatch(typeof(ManufacturingMachineInteractive), "ActivateMachine")]
public class SpecialItemsAndSkullsPatch
{
    static void Prefix(ref ManufacturingMachineInteractive __instance)
    {
        // Pretty!
        ref var self = ref __instance;

        // A list of all hidden/special stuff
        // Still want: a better generic solution
        var specialItemsAndSkulls = new string[]{
            // Skulls known to work
            "PlagueDoctor",

            // Hidden items

            // Quintessences
            "Kiriz",
        };

        // Now we add them into their respective machines!
        // Auto-formatter sucks for switches, so let's do an if-else chain :P
        if (self._type == Gear.Type.Weapon)
        {
            self._gearList.AddRange(GearResource.instance.weapons.Where(
                (WeaponReference weapon) => specialItemsAndSkulls.Contains(weapon.name))
            );
        }

        if (self._type == Gear.Type.Quintessence)
        {
            self._gearList.AddRange(GearResource.instance.essences.Where(
                (EssenceReference essence) => specialItemsAndSkulls.Contains(essence.name))
            );
        }

        if (self._type == Gear.Type.Item)
        {
            self._gearList.AddRange(GearResource.instance.items.Where(
                (ItemReference item) => specialItemsAndSkulls.Contains(item.name))
            );
        }
    }
}
