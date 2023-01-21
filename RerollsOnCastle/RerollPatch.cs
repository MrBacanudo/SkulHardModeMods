using Characters;
using Characters.Gear.Weapons;
using Hardmode.Darktech;
using HarmonyLib;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace RerollsOnCastle;

[HarmonyPatch(typeof(DarktechMachine), "InteractWith")]
public class RerollPatch
{
    static bool Prefix(ref DarktechMachine __instance)
    {
        ref var self = ref __instance;

        if (self._darktech.type != DarktechData.Type.ItemRotationEquipment)
        {
            return true;
        }

        // If interacting with the Item Rotation, we'll instead rotate our skills!
        LevelManager levelManager = Singleton<Service>.Instance.levelManager;
        levelManager.player.playerComponents.inventory.weapon.current.RerollSkills();
        return false;
    }
}
