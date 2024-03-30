using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Gear;
using Characters.Gear.Items;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using GameResources;
using HarmonyLib;
using Level;
using UnityEngine;

namespace CustomItems;

[HarmonyPatch]
public class CustomItemsPatch
{
    static Dictionary<ItemRequest, CustomItemReference> currentRequests = new();
    static Dictionary<GearRequest, CustomItemReference> currentGearRequests = new();

    public delegate void ManatechPartDelegate();
    public static event ManatechPartDelegate OnManatechPart;

    public delegate void InventoryUpdateDelegate(Inventory inventory);
    public static event InventoryUpdateDelegate OnInventoryUpdate;
    public static event InventoryUpdateDelegate AfterInventoryUpdate;

    private static Dictionary<string, string> ItemStrings = new();

    // This should allow other mods to translate the items contained here.
    // If you want to provide one, ensure this runs after InjectCustomItems.
    // A guaranteed way should be postfixing the method we prefix with it.
    public static readonly Dictionary<string, string> ItemStringsOverride = new();

    // Inject our custom items to the game's list of items.
    // This only runs once, at the beginning of the game. So loading a save should also work.
    [HarmonyPrefix]
    [HarmonyPatch(typeof(GearResource), "Initialize")]
    static void InjectCustomItems(ref GearResource __instance)
    {
        ref var self = ref __instance;

        // Load custom items
        // Using method calls instead of static arrays so other mods can inject new items before initialization
        ItemReference[] customItems = CustomItems.Items.ToArray();
        CustomItems.LoadSprites();
        ItemStrings = CustomItems.MakeStringDictionary();

        // Extend the items array to fit our custom items
        var oldLength = self._items.Length;
        Array.Resize(ref self._items, oldLength + customItems.Length);

        // Copy the custom items to the tail of the array
        customItems.CopyTo(self._items, oldLength);

        // Inject thumbnails, if they exist
        var thumbnails = customItems.Where((item) => item.thumbnail != null).Select((item) => item.thumbnail).ToArray();
        var oldThumbnailCount = self._gearThumbnails.Length;
        Array.Resize(ref self._gearThumbnails, oldThumbnailCount + thumbnails.Length);
        thumbnails.CopyTo(self._gearThumbnails, oldThumbnailCount);
    }

    // Inject our custom masterpiece items to the masterpiece map
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Masterpiece), "Initialize")]
    static void InjectMasterpieces(ref Masterpiece __instance)
    {
        ref var self = ref __instance;

        // Custom items
        var customItems = CustomItems.ListMasterpieces().ToArray();

        // Extend the items array to fit our custom items
        var oldLength = self._enhancementMaps.Length;
        Array.Resize(ref self._enhancementMaps, oldLength + customItems.Length);

        // Copy the custom items to the tail of the array
        customItems.CopyTo(self._enhancementMaps, oldLength);
    }

    // Inject our custom upgradable Bone items to the Bone map
    private static bool _initializedBone = false;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Bone.SuperAbility), "CreateInstance")]
    static void InjectUpgradedBones(ref Bone.SuperAbility __instance)
    {
        if (_initializedBone)
        {
            return;
        }

        ref var self = ref __instance;

        // Custom items
        var customItems = CustomItems.ListUpgradableBones().ToArray();

        // Extend the items array to fit our custom items
        var oldLength = self._enhancementMaps.Length;
        Array.Resize(ref self._enhancementMaps, oldLength + customItems.Length);

        // Copy the custom items to the tail of the array
        customItems.CopyTo(self._enhancementMaps, oldLength);

        _initializedBone = true;
    }

    // Register a custom item into this class, so we remember it's actually a custom item when dropping the placeholder.
    // The request was the result of the LoadAsync method, and it's just the placeholder item, so we have something to load.
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemReference), "LoadAsync")]
    static void RegisterGearRequest(ref ItemReference __instance, ref ItemRequest __result)
    {
        if (__instance is CustomItemReference customItem)
        {
            currentRequests.Add(__result, customItem);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GearReference), "LoadAsync")]
    static void RegisterGearRequest(ref GearReference __instance, ref GearRequest __result)
    {
        if (__instance is CustomItemReference customItem)
        {
            currentGearRequests.Add(__result, customItem);
        }
    }

    // Intercept each call to DropItem with a result from LoadAsync.
    // If the item is one of our placeholders, we instead just drop the correct item.
    [HarmonyPrefix]
    [HarmonyPatch(typeof(LevelManager), "DropItem", new Type[] { typeof(ItemRequest), typeof(Vector3) })]
    static bool DropCustomItem(ref LevelManager __instance, ItemRequest gearRequest, Vector3 position, ref Item __result)
    {
        return ProcessDrop(ref __instance, gearRequest, position, ref __result, currentRequests);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(LevelManager), "DropGear", new Type[] { typeof(GearRequest), typeof(Vector3) })]
    static bool DropCustomGear(ref LevelManager __instance, GearRequest gearRequest, Vector3 position, ref Gear __result)
    {
        return ProcessDrop(ref __instance, gearRequest, position, ref __result, currentGearRequests);
    }

    private static bool ProcessDrop<TRequest, TResult>(ref LevelManager __instance,
                                                       TRequest request, Vector3 position,
                                                       ref TResult __result,
                                                       Dictionary<TRequest, CustomItemReference> requestDict)
        where TResult : Gear
        where TRequest : Request<TResult>
    {
        // We leave it to the game in case it's not a custom item
        if (!requestDict.ContainsKey(request))
        {
            return true;
        }

        var customItem = requestDict[request];

        // Now we drop our item and skip the regular process
        var originalItem = customItem.GetItem(request.asset.gameObject.GetComponent<Item>());
        __result = (TResult)(Gear)__instance.DropItem(originalItem, position);
        requestDict.Remove(request);

        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Localization), "GetLocalizedString", new Type[] { typeof(string) })]
    static bool InjectCustomStrings(string key, ref string __result)
    {
        if (ItemStringsOverride.ContainsKey(key))
        {
            __result = ItemStringsOverride[key];
            return false;
        }

        if (!ItemStrings.ContainsKey(key))
        {
            return true;
        }

        __result = ItemStrings[key];
        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(DroppedManatechPart), "PickedUpBy")]
    static void ManatechPickedupHook(Characters.Character character)
    {
        OnManatechPart?.Invoke();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), "UpdateSynergy")]
    static void InventoryUpdateHook(ref Inventory __instance)
    {
        OnInventoryUpdate?.Invoke(__instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Inventory), "UpdateSynergy")]
    static void PostInventoryUpdateHook(ref Inventory __instance)
    {
        AfterInventoryUpdate?.Invoke(__instance);
    }
}
