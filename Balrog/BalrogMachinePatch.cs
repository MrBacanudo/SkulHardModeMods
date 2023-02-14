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

namespace Balrog;

[HarmonyPatch(typeof(ManufacturingMachineInteractive), "ActivateMachine")]
public class BalrogMachinePatch
{
    static void Prefix(ref ManufacturingMachineInteractive __instance)
    {
        ref var self = ref __instance;

        if (self._type == Gear.Type.Weapon)
        {
            self._gearList.AddRange(GearResource.instance.weapons.Where(
                (WeaponReference weapon) => (weapon.name == "Balrog_Head"))
            );
        }
    }
}
