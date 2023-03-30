using System.Collections.Generic;
using System.Reflection;
using Characters.Gear;
using GameResources;
using Hardmode.Darktech;
using HarmonyLib;
using Services;
using Singletons;
using UnityEngine;

namespace RandomStart;

[HarmonyPatch(typeof(ManufacturingMachineInteractive))]
public class RandomPatch
{
    static readonly Sprite sprite = MakeSprite();
    static readonly System.Random random = new();

    static bool active = false;

    internal class FakeReference : GearReference
    {
        internal Gear.Type _type;
        public override Gear.Type type => _type;
    }

    [HarmonyPrefix]
    [HarmonyPatch("ActivateMachine")]
    static void ActiveMachinePrefix(ref ManufacturingMachineInteractive __instance)
    {
        ref var self = ref __instance;

        FakeReference reference = new();
        reference._type = self._type;

        reference.icon = sprite;

        self._gearList.Insert(0, reference);

        active = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch("Select")]
    static void SelectPrefix(ref ManufacturingMachineInteractive __instance)
    {
        ref var self = ref __instance;

        if (self._running || self._currentIndex != 0)
        {
            return;
        }

        active = true;
        self._currentIndex = random.Next(1, self._gearList.Count);
    }

    [HarmonyPrefix]
    [HarmonyPatch("Down")]
    static void DownPrefix(ref ManufacturingMachineInteractive __instance)
    {
        ref var self = ref __instance;
        if (!self._running && active)
        {
            self._currentIndex = 1;
            self.Load();
            active = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch("Select")]
    static void SelectPostfix(ref ManufacturingMachineInteractive __instance)
    {
        ref var self = ref __instance;
        if (!active)
        {
            return;
        }

        active = false;

        if (self._currentIndex != 0)
        {
            self._currentIndex = 1;
            self.Load();
            self.Down();
        }
    }

    static Sprite MakeSprite()
    {
        var assembly = typeof(RandomPatch).Assembly;
        var resource = assembly.GetManifestResourceStream("RandomStart.QuestionMark.png");

        byte[] buf = new byte[resource.Length];
        resource.Read(buf, 0, (int)resource.Length);

        Texture2D texture = new(2, 2);
        texture.LoadImage(buf);
        texture.filterMode = FilterMode.Point;
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 600.0f);
    }
}
