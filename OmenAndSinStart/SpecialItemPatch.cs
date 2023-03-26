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

namespace OmenAndSinStart;

[HarmonyPatch(typeof(ManufacturingMachineInteractive), "ActivateMachine")]
public class SpecialItemPatch
{
    static void Prefix(ref ManufacturingMachineInteractive __instance)
    {
        // Pretty!
        ref var self = ref __instance;

        // Only works on items
        if (self._type != Gear.Type.Item)
        {
            return;
        }

        // A list of all boss and adventurer items
        // Want: a better generic solution
        var specialItems = new string[]{
            // Boss Items
            "ElderEntsGratitude",
            "UnknownSeed",
            "GoldenManeRapier",
            "ProofOfFellowship",
            "ContaminatedCore",
            "ChimerasFang",
            "GraceOfLeonia",
            "ArchbishopsBible",

            // Adventurer Items
            "TheChosenClericsBible",
            "TheChosenHerosCirclet",
            "TheChosenHuntersArrow",
            "TheChosenMagesBadge",
            "TheChosenThiefsTwinSwords",
            "TheChosenWarriorsArmor",

            // Slime's Item!
            "MagicalWand",
        };

        // Add by order, so they're always grouped
        // 1. Special Items
        self._gearList.AddRange(GearResource.instance.items.Where(
            (ItemReference item) => specialItems.Contains(item.name))
        );

        // 2. Sin Items
        self._gearList.AddRange(GearResource.instance.items.Where(
            (ItemReference item) => (item.prefabKeyword1 == Inscription.Key.Sin || item.prefabKeyword2 == Inscription.Key.Sin)
        ));

        // 3. Omen Items
        self._gearList.AddRange(GearResource.instance.items.Where(
            (ItemReference item) => (item.gearTag.HasFlag(Gear.Tag.Omen) && !item.gearTag.HasFlag(Gear.Tag.UpgradedOmen))
        ));
    }
}
