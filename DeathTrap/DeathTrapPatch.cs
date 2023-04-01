using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Gear.Weapons.Gauges;
using HarmonyLib;
using UnityEngine;

namespace DeathTrap;

[HarmonyPatch(typeof(Weapon), "InitializeSkills")]
public class DeathTrapPatch
{
    static void Prefix(ref Weapon __instance)
    {
        ref var self = ref __instance;
        var lich = self.gameObject;

        // Only modify Lich
        if (!lich.name.StartsWith("ArchLich"))
        {
            return;
        }

        // Remove the gauge
        self._gauge = null;
        Component.Destroy(self.GetComponent<ValueGauge>());

        // Remove the Gauge filler
        self._abilityAttacher._components = self._abilityAttacher._components.Where((attacher, idx) => (idx != 2)).ToArray();

        // Get Death Trap
        var deathtrap = lich.transform.Find("Equipped/DeadLock").gameObject;

        // Remove the "press up" requirement
        var action = deathtrap.GetComponent<SimpleAction>();
        action._type = Characters.Actions.Action.Type.Skill;
        action._constraints._components = action._constraints._components.Where((attacher, idx) => (idx != 1)).ToArray();

        // Replace the gauge with a timer
        action._cooldown = new Characters.Cooldowns.CooldownSerializer
        {
            _cooldownTime = 45,
            _type = Characters.Cooldowns.CooldownSerializer.Type.Time,
            _maxStack = 1,
        };

        // Set Death Trap as a regular skill
        var skill = deathtrap.AddComponent<SkillInfo>();
        skill._hasAlways = true;
        skill._key = "DeadLock";
        skill.action = action;
    }
}
