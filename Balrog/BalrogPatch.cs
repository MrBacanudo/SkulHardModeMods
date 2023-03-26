using HarmonyLib;
using Characters.Gear.Weapons;
using static Characters.Damage;
using UnityEngine;
using Characters.Gear.Weapons.Gauges;
using Characters.Player;
using Singletons;
using Characters;
using Services;
using Characters.Operations.Summon;

namespace Balrog;

[HarmonyPatch(typeof(Weapon), "OnEquipped")]
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

        // Only work with Balrog, of course
        if (self.name != "Balrog_Head")
        {
            return;
        }

        // Allow the Balorg quintessence to deal quintessence damage
        Character player = Singleton<Service>.Instance.levelManager.player;
        if (player.playerComponents.inventory.weapon.polymorphWeapon == self)
        {
            return;
        }

        // Prevent redoing this multiple times by tagging the object with a 
        const string dummyName = "Balrog_Mod_Tag";
        if (self.gameObject.transform.Find(dummyName) != null)
        {
            return;
        }

        GameObject tag = new(dummyName);
        tag.transform.parent = self.gameObject.transform;

        // Make most damage sources deal non-quintessence damage
        SetType(self.gameObject, "Equipped/Dash_Tackle", MotionType.Dash);
        SetType(self.gameObject, "Equipped/PowerbombJumpAttack", MotionType.Basic);
        SetType(self.gameObject, "Equipped/JumpAttack", MotionType.Basic);
        SetType(self.gameObject, "Equipped/Skill_Catastrophe", MotionType.Skill);
        SetType(self.gameObject, "Equipped/ComboAttack(3)", MotionType.Basic);
        SetType(self.gameObject, "Equipped/ComboAttack(4) (1)", MotionType.Basic);

        // Make the spawned effects deal skill and basic attack damage
        foreach (var runner in self.gameObject.transform.Find("Equipped/ComboAttack(3)").GetComponentsInChildren<SummonOperationRunner>())
        {
            var poolObj = runner._operationRunner._poolObject;
            var obj = poolObj.gameObject;
            var name = obj.name;

            if (name != "Balrog_BasicAttack" && name != "Balrog_lava")
            {
                // Prevent creating a new unnecessary object by duplicating conditionals, because why not?
                continue;
            }

            // Clone object, so the quintessence isn't changed
            var clonedPoolObj = UnityEngine.Object.Instantiate<PoolObject>(poolObj);
            UnityEngine.Object.DontDestroyOnLoad(clonedPoolObj);
            poolObj._keepOriginal = true;
            var clonedObj = clonedPoolObj.gameObject;
            runner._operationRunner = clonedObj.GetComponent<OperationRunner>();
            runner._operationRunner._poolObject = clonedPoolObj;

            if (name == "Balrog_BasicAttack")
            {
                SetTypeInChildren(clonedObj, MotionType.Basic);
            }
            else if (name == "Balrog_lava")
            {
                // TODO: figure out the actual damage source of lava. This does nothing.
                SetTypeInChildren(clonedObj, MotionType.Skill);
            }

            clonedObj.SetActive(false);
        }

        foreach (var runner in self.gameObject.transform.Find("Equipped/Skill_Catastrophe").GetComponentsInChildren<SummonOperationRunnersOnGround>())
        {
            var poolObj = runner._operationRunner._poolObject;
            var obj = poolObj.gameObject;
            var name = obj.name;

            if (name == "Balrog_Skill")
            {
                var clonedPoolObj = UnityEngine.Object.Instantiate<PoolObject>(poolObj);
                UnityEngine.Object.DontDestroyOnLoad(clonedPoolObj);
                poolObj._keepOriginal = true;
                var clonedObj = clonedPoolObj.gameObject;
                runner._operationRunner = clonedObj.GetComponent<OperationRunner>();
                runner._operationRunner._poolObject = clonedPoolObj;

                SetTypeInChildren(clonedObj, MotionType.Skill);
                clonedObj.SetActive(false);
            }
        }

        var gauge = self.gameObject.GetComponent<ValueGauge>();
        if (gauge != null)
        {
            Object.Destroy(gauge);
        }
    }
}
