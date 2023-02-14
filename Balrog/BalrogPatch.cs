using HarmonyLib;
using Characters.Gear.Weapons;
using static Characters.Damage;
using UnityEngine;
using Characters.Gear.Weapons.Gauges;

namespace Balrog;

[HarmonyPatch(typeof(Weapon), "OnLoot")]
public class BalrogPatch
{
    private static void SetTypeInChildren(GameObject self, MotionType type)
    {
        var attacks = self?.GetComponentsInChildren<Characters.Operations.Attack.SweepAttack>();
        if (attacks == null)
        {
            return;
        }
        foreach (var attack in attacks)
        {
            attack._hitInfo._motionType = type;
        }
    }
    private static void SetType(GameObject self, string name, MotionType type)
    {
        var attacks = self.gameObject.transform.Find(name)?.GetComponentsInChildren<Characters.Operations.Attack.SweepAttack>();
        if (attacks == null)
        {
            return;
        }
        foreach (var attack in attacks)
        {
            attack._hitInfo._motionType = type;
        }
    }

    static void Prefix(ref Weapon __instance)
    {
        ref var self = ref __instance;
        if (self.name != "Balrog_Head")
        {
            return;
        }

        SetType(self.gameObject, "Equipped/Dash_Tackle", MotionType.Dash);
        SetType(self.gameObject, "Equipped/PowerbombJumpAttack", MotionType.Basic);
        SetType(self.gameObject, "Equipped/JumpAttack", MotionType.Basic);
        SetType(self.gameObject, "Equipped/Skill_Catastrophe", MotionType.Skill);
        SetType(self.gameObject, "Equipped/ComboAttack(3)", MotionType.Basic);
        SetType(self.gameObject, "Equipped/ComboAttack(4) (1)", MotionType.Basic);

        // TODO: find a way that doesn't ruin using Balrog Quintessence after using this once
        // A possibility is instantiating all pool objects, then using the referenced clones
        // Just reset the game for now!
        foreach (var runner in self.gameObject.transform.Find("Equipped/ComboAttack(3)").GetComponentsInChildren<Characters.Operations.Summon.SummonOperationRunner>())
        {
            var obj = runner._operationRunner._poolObject.gameObject;
            var name = obj.name;
            if (name == "Balrog_BasicAttack")
            {
                SetTypeInChildren(obj, MotionType.Basic);
            }
            else if (name == "Balrog_lava")
            {
                // TODO: figure out the actual damage source of lava. This does nothing.
                SetTypeInChildren(obj, MotionType.Skill);
            }
        }

        foreach (var runner in self.gameObject.transform.Find("Equipped/Skill_Catastrophe").GetComponentsInChildren<Characters.Operations.Summon.SummonOperationRunnersOnGround>())
        {
            var obj = runner._operationRunner._poolObject.gameObject;
            var name = obj.name;
            if (name == "Balrog_Skill")
            {
                SetTypeInChildren(obj, MotionType.Skill);
            }
        }

        var gauge = self.gameObject.GetComponent<ValueGauge>();
        if (gauge != null)
        {
            Object.Destroy(gauge);
        }
    }
}
