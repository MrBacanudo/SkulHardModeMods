using Characters.Gear.Items;
using Data;
using HarmonyLib;
using Level;
using Level.Npc.FieldNpcs;
using Services;
using Singletons;

namespace FixNPCs;

[HarmonyPatch]
public class FixNPCPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(FogWolf), "Start")]
    static void FixSpokesmanRNG(ref FogWolf __instance)
    {
        __instance._random.Next();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MagicalSlime), "CPolymorphToRandomItem")]
    static void FixSlimeRNG(ref MagicalSlime __instance, ref Item targetItem)
    {
        // RNG method based on how the game actually works. Could do it using transpilers instead.
        Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
        System.Random random = new System.Random(
            GameData.Save.instance.randomSeed
            + MagicalSlime._randomSeed
            + (int)currentChapter.type * 256
            + currentChapter.stageIndex * 16
            + currentChapter.currentStage.pathIndex
        );

        // This is the only change we actually do
        random.Next();

        // Then inject the new true random item
        targetItem = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item.GetRandomItem(random);
    }
}
