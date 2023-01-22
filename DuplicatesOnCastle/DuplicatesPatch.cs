using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Characters;
using Characters.Gear.Weapons;
using Hardmode.Darktech;
using HarmonyLib;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace DuplicatesOnCastle;

[HarmonyPatch(typeof(ManufacturingMachineInteractive), "Select")]
public class DuplicatesPatch
{
    static FieldInfo gearListField = AccessTools.Field(typeof(ManufacturingMachineInteractive), nameof(ManufacturingMachineInteractive._gearList));
    static FieldInfo currentIndexField = AccessTools.Field(typeof(ManufacturingMachineInteractive), nameof(ManufacturingMachineInteractive._currentIndex));
    static MethodInfo downMethod = typeof(ManufacturingMachineInteractive).GetMethod("Down");

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        // We'll remove the two lines of code that protect duplicates on the machines.
        // For that, we'll match the 7 lines of IL that do it, and just remove them.
        return new CodeMatcher(instructions).MatchForward(false,
            new CodeMatch(OpCodes.Ldarg_0),
            new CodeMatch(inst => inst.LoadsField(gearListField)),
            new CodeMatch(OpCodes.Ldarg_0),
            new CodeMatch(inst => inst.LoadsField(currentIndexField)),
            new CodeMatch(OpCodes.Callvirt), // Calls Remove(). Should we put it here?
            new CodeMatch(OpCodes.Ldarg_0),
            new CodeMatch(OpCodes.Call, downMethod)
        ).RemoveInstructions(7)
         .InstructionEnumeration();
    }
}
