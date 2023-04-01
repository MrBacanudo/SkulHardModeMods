using System;
using System.Reflection;
using Characters.Abilities.Customs;
using Characters.Gear.Weapons;
using Characters.Player;
using Hardmode.Darktech;
using HarmonyLib;

namespace ThreeSkulls;

[HarmonyPatch]
public class ThreeSkullsPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(WeaponInventory), "Awake")]
    static void AllowThreeSkulls(ref WeaponInventory __instance)
    {
        ref var self = ref __instance;
        Weapon[] weapons = self.weapons;
        if (weapons.Length == 2)
        {
            FieldInfo weaponsField = AccessTools.Field(typeof(WeaponInventory), nameof(WeaponInventory.weapons));
            weaponsField.SetValue(self, new Weapon[3] { weapons[0], weapons[1], null });
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ManufacturingMachineInteractive), "Start")]
    static void StartWithThreeSkulls(ref ManufacturingMachineInteractive __instance)
    {
        ref var self = ref __instance;

        if (self._type == Characters.Gear.Gear.Type.Weapon)
        {
            self._selectCount = 3;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BoneOfMana.Instance), "UpdateBalanaceHeadCount")]
    static void FixBoneOfMana(ref BoneOfMana.Instance __instance)
    {
        __instance._balanceHeadCount = Math.Min(__instance._balanceHeadCount, 2);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BoneOfBrave.Instance), "UpdatePowerHeadCount")]
    static void FixBoneOfCourage(ref BoneOfBrave.Instance __instance)
    {
        __instance._powerHeadCount = Math.Min(__instance._powerHeadCount, 2);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BoneOfSpeed.Instance), "UpdateSpeedHeadCount")]
    static void FixBoneOfSpeed(ref BoneOfSpeed.Instance __instance)
    {
        __instance._speedHeadCount = Math.Min(__instance._speedHeadCount, 2);
    }
}
