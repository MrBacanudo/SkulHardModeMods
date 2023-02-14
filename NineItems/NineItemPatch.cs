using Characters;
using Characters.Gear.Weapons;
using Hardmode.Darktech;
using HarmonyLib;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace NineItems;

[HarmonyPatch(typeof(ManufacturingMachineInteractive), "Start")]
public class NineItemPatch
{
    static void Prefix(ref ManufacturingMachineInteractive __instance)
    {
        ref var self = ref __instance;

        if (self._type != Characters.Gear.Gear.Type.Item)
        {
            return;
        }

        // Allow users to set how many items they want.
        self._selectCount = Mathf.Max(0, Plugin.MaxItems.Value);
    }
}
