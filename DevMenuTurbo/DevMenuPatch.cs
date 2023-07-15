using TMPro;
using UI.TestingTool;
using HarmonyLib;
using UnityEngine;
using GameResources;
using Singletons;
using Services;
using Level;
using Characters.Gear.Items;
using UnityEngine.UI;

namespace DevMenu;

[HarmonyPatch]
public class DevMenuTurboPatch
{
    [HarmonyPatch(typeof(Panel), "canUse", MethodType.Getter)]
    static bool Prefix(ref Panel __instance, ref bool __result)
    {
        __result = true;
        return false;
    }

    [HarmonyPatch(typeof(Panel), "Awake")]
    [HarmonyPostfix]
    static void TranslateToEnglish(ref Panel __instance)
    {
        ref var self = ref __instance;

        SetText(self._openMapList, "Maps");
        SetText(self._nextStage, "Next Stage");
        SetText(self._nextMap, "Next Map");
        SetText(self._openGearList, "Gear List");
        SetText(self._hideUI, "Hide UI");
        SetText(self._getGold, "+10k Gold");
        SetText(self._getDarkquartz, "+1k Quartz");
        SetText(self._getBone, "+100 Bones");
        SetText(self._getHeartQuartz, "+100 Core Quartz");
        SetText(self._testMap, "Test Map");
        SetText(self._awake, "Awaken");
        SetText(self._damageBuff, "DMG Buff");
        SetText(self._hp10k, "10k HP");
        SetText(self._noCooldown, "No Cooldown");
        SetText(self._shield10, "+10 Shield");
        SetText(self._hardmodeToggle, "Hard Mode");
        SetText(self._rerollSkill, "Reroll Skills");
        SetText(self._timeScaleReset, "Reset");
        SetText(self._infiniteRevive, "Immortal");
        SetText(self._verification, "Map Heals");
        SetText(self._right3, "All 3 buffs ->");

        SetText2(self._hardmodeLevelSlider, "DM Level");
        SetText2(self._hardmodeClearedLevelSlider, "DM Unlocked");
        SetText2(self._timeScaleSlider, "Time Scale");

        SetText(self._gearList.transform, "Return");

        // Disable showing your time zone, just in case
        self._localNow.gameObject.SetActive(false);
        self._utcNow.gameObject.SetActive(false);
    }

    private static void SetText(Component obj, string text)
    {
        obj.GetComponentInChildren<TextMeshProUGUI>(true)?.SetText(text);
    }

    private static void SetText2(Component obj, string text)
    {
        var texts = obj.GetComponentsInChildren<TextMeshProUGUI>(true);
        if (texts != null && texts.Length > 1)
        {
            texts[1].SetText(text);
        }
    }

    [HarmonyPatch(typeof(GearListElement), "Set")]
    [HarmonyPostfix]
    static void FillInventoryOnClick(ref GearListElement __instance, GearReference gearReference)
    {
        ref var self = ref __instance;

        if (gearReference.type != Characters.Gear.Gear.Type.Item)
            return;

        var handler = self.gameObject.AddComponent<ButtonRightClickHandler>();
        handler.OnRightClick += delegate
        {
            GearRequest request = gearReference.LoadAsync();
            request.WaitForCompletion();

            LevelManager manager = Singleton<Service>.Instance.levelManager;
            var inventory = manager.player.playerComponents.inventory.item;

            Item item = null;

            for (int i = 0; i < inventory.items.Count; i++)
            {
                if (inventory.items[i] == null)
                {
                    if (item == null)
                    {
                        item = (Item)manager.DropGear(request, Vector3.zero);
                        inventory.EquipAt(item, i);
                    }
                    else
                    {
                        Item clone = manager.DropItem(item, Vector3.zero);
                        inventory.EquipAt(clone, i);
                    }
                }
            }
        };
    }

    [HarmonyPatch(typeof(GearList), "Awake")]
    [HarmonyPostfix]
    static void FixDarkAbilitySearch(ref GearList __instance)
    {
        var self = __instance;
        foreach (var button in self._upgradeListElements)
        {
            Text name = button.GetComponentInChildren<Text>();
            button.name = name.text;
            name.fontSize = 29;
        }

        self._inputField.onValueChanged.RemoveAllListeners();
        self._inputField.onValueChanged.AddListener(delegate
        {
            if (self._currentFilter == GearList.Filter.Upgrade)
                self.FilterUpgrade();
            else
                self.FilterGearList();
        });
    }
}
