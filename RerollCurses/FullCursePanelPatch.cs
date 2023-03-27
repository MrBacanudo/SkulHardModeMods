using System.Collections.Generic;
using System.Linq;
using Characters.Gear.Upgrades;
using HarmonyLib;
using InControl;
using Services;
using Singletons;
using UI.Upgrades;
using UnityEngine;
using UserInput;

namespace RerollCurses;

[HarmonyPatch(typeof(UpgradableContainer))]
public class FullCursePanelPatch
{

    [HarmonyPrefix]
    [HarmonyPatch("CreateElements")]
    static bool CreateElementsPrefix(ref UpgradableContainer __instance)
    {
        if (!RerollCursePanelPatch.cursePage)
        {
            return true;
        }

        var self = __instance;

        self._upgradeElements.Clear();

        var curses = Singleton<UpgradeManager>.Instance._upgrades.Where((obj) => obj.type == UpgradeObject.Type.Cursed).ToList();
        curses.Sort((ref1, ref2) => ref1.displayName.CompareTo(ref2.displayName));

        foreach (var curse in curses)
        {
            var section = (self._upgradeElements.Count < 2) ? UpgradeObject.Type.Cursed : UpgradeObject.Type.Normal;
            UpgradeElement element = Object.Instantiate(self._elementPrefab, self._elementParents[section]);
            element.Initialize(curse, self._panel);
            self._upgradeElements.Add(element);
        }

        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch("GetDefaultFocusTarget")]
    static bool GetDefaultFocusTargetPrefix(ref UpgradableContainer __instance, ref UpgradeElement __result)
    {
        if (!RerollCursePanelPatch.cursePage)
        {
            return true;
        }

        __result = __instance._upgradeElements.Find((element) => (
            element != null
            && !Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.Has(element._reference)
            && element._button.interactable
        ));

        return false;
    }
}
