using System;
using System.Collections.Generic;
using System.Reflection;
using Characters.Gear;
using GameResources;
using Hardmode.Darktech;
using HarmonyLib;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace QuintessenceStart;

[HarmonyPatch(typeof(LevelManager), "InvokeOnMapChanged")]
public class QuintessencePatch
{
    static void Prefix(ref LevelManager __instance)
    {
        ref var self = ref __instance;

        // We don't care about rooms that are not the Dark Castle
        if (self.currentChapter.type != Chapter.Type.HardmodeCastle)
        {
            return;
        }

        //Singleton<Scenes.Scene>.

        // Try to find a quintessence machine. If there's none, we need to create one.
        var quintMachine = GameObject.Find("QuintessenceManufacturingMachine");

        if (quintMachine == null)
        {
            var itemMachine = GameObject.Find("ItemManufacturingMachine");

            if (itemMachine == null || itemMachine.GetComponentInChildren<ManufacturingMachineInteractive>() == null)
            {
                Debug.LogError("StartingQuintessence mod: Could not find an Item Machine to clone.");
                return;
            }

            quintMachine = (GameObject)GameObject.Instantiate(itemMachine, itemMachine.transform.parent);
            quintMachine.name = "QuintessenceManufacturingMachine";
            quintMachine.transform.position = itemMachine.transform.position - new Vector3(3.45f, 0, 0);

            var machine = quintMachine.GetComponentInChildren<ManufacturingMachineInteractive>();
            machine._type = Gear.Type.Quintessence;
            machine._selectCount = 1;
            machine._remainSelectCount = 1;
        }

    }
}
