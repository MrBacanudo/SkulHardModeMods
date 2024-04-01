using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Characters.Abilities;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Gear.Weapons.Gauges;
using Characters.Operations.Gauge;
using HarmonyLib;
using UnityEngine;
using Action = Characters.Actions.Action;

namespace BuffChamp;

[HarmonyPatch]
public class BuffChampPatch
{

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Weapon), "InitializeSkills")]
    static void ReduceSkillCosts(ref Weapon __instance)
    {
        ref var self = ref __instance;
        var champ = self.gameObject;

        // Only modify Champ
        if (!champ.name.StartsWith("Fighter"))
        {
            return;
        }

        try
        {
            var skills = champ.GetComponentsInChildren<SkillInfo>(includeInactive: true);

            foreach (var skill in skills)
            {

                var action = skill.GetComponent<Action>();
                var costEffects = skill.GetComponentsInChildren<AddGaugeValue>(includeInactive: true);

                const int amount = 10;

                action.cooldown._requiredAmount -= amount;

                // We need to find the one that represents the cost, because Champ's skills have operations that refund energy
                foreach (var costEffect in costEffects)
                {
                    if (costEffect._amount >= 0)
                        continue;
                    costEffect._amount += amount;
                    break;
                }

            }
        }
        catch (Exception e)
        {
            Debug.LogError("[BuffChamp]: " + e);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Weapon), "StartSwitchAction")]
    static void RestoreEnergyOnSwap(ref Weapon __instance)
    {
        ref var self = ref __instance;
        var champ = self.gameObject;

        // Only modify Champ
        if (!champ.name.StartsWith("Fighter"))
        {
            return;
        }

        var gauge = self._gauge as ValueGauge;
        gauge?.Add(25);

        var passive = champ.GetComponent<FighterPassiveAttacher>()._fighterPassive;
        passive.UpdateTime(10);
    }

}
