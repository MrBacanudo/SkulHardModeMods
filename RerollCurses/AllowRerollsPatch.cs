using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Characters.Gear.Upgrades;
using HarmonyLib;
using UnityEngine;

namespace RerollCurses;

// This patch allows us to use a single random instance, to get multiple 

[HarmonyPatch(typeof(UpgradeShop), "LoadCusredLineUp")]
public class AllowRerollsPatch
{
    static FieldInfo randomField = AccessTools.Field(typeof(UpgradeShop), nameof(UpgradeShop._random));

    static void Prefix(ref UpgradeShop __instance)
    {
        ref var self = ref __instance;

        if (self._random == null)
        {
            self._random = new System.Random();
        }
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            if (instruction.StoresField(randomField))
            {
                yield return new CodeInstruction(OpCodes.Pop);
                yield return new CodeInstruction(OpCodes.Pop);
            }
            else
            {
                yield return instruction;
            }
        }
    }
}
