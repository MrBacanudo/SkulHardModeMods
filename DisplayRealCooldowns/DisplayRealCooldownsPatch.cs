using UI.TestingTool;
using HarmonyLib;
using UI.Inventory;
using Characters.Gear.Quintessences;
using UnityEngine;
using Characters.Gear.Weapons;
using Characters.Cooldowns;

namespace DisplayRealCooldowns;

[HarmonyPatch]
public class DisplayRealCooldownsPatch
{
    private static Color RGBColor(int r, int g, int b) => new Color(r / 255.0f, g / 255.0f, b / 255.0f);

    [HarmonyPostfix]
    [HarmonyPatch(typeof(QuintessenceOption), "Set")]
    static void DisplayRealQuintCooldown(ref QuintessenceOption __instance, ref Quintessence essence)
    {
        ref var self = ref __instance;

        var time = essence.cooldown.time;
        var realTime = (time.cooldownTime / time.GetCooldownSpeed());
        self._cooldown.text = realTime.ToString(realTime > 9.9 ? "0" : "0.#");

        if (time.ToString() == self._cooldown.text)
        {
            self._cooldown.color = RGBColor(134, 97, 90);
            return;
        }

        self._cooldown.color = time.GetCooldownSpeed() switch
        {
            > 1.0f => RGBColor(82, 143, 67),
            < 1.0f => RGBColor(186, 51, 44),
            _ => RGBColor(134, 97, 90),
        };
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SkillOption), "Set")]
    static void DisplayRealSkillCooldown(ref SkillOption __instance, ref SkillInfo skillDescInfo)
    {
        ref var self = ref __instance;

        if (self._cooldown == null)
        {
            return;
        }

        var cooldown = skillDescInfo.action.cooldown;

        if (cooldown.type != CooldownSerializer.Type.Time)
        {
            return;
        }

        var time = cooldown.time;
        var realTime = (time.cooldownTime / time.GetCooldownSpeed());
        self._cooldown._text.text = realTime.ToString(realTime > 9.9 ? "0" : "0.#");

        if (time.ToString() == self._cooldown._text.text)
        {
            self._cooldown._text.color = RGBColor(68, 51, 68);
            return;
        }

        self._cooldown._text.color = time.GetCooldownSpeed() switch
        {
            > 1.0f => RGBColor(37, 149, 33),
            < 1.0f => RGBColor(186, 24, 33),
            _ => RGBColor(68, 51, 68),
        };
    }
}
