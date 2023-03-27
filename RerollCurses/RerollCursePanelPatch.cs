using HarmonyLib;
using InControl;
using UI.Upgrades;
using UnityEngine;
using UserInput;

namespace RerollCurses;

[HarmonyPatch(typeof(Panel))]
public class RerollCursePanelPatch
{
    static readonly Sprite sprite = MakeSprite();
    internal static bool cursePage = false;

    [HarmonyPrefix]
    [HarmonyPatch("Update")]
    static void UpdatePrefix(ref Panel __instance)
    {
        ref var self = ref __instance;
        if (self.focused && (Input.GetKeyDown(KeyCode.Tab) ||
            InputManager.ActiveDevice.GetControl(InputControlType.RightBumper).WasPressed))
        {
            cursePage = !cursePage;
            if (!cursePage)
            {
                Singletons.Singleton<Characters.Gear.Upgrades.UpgradeShop>.Instance.LoadCusredLineUp();
            }
            self.UpdateAll();
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch("Awake")]
    static void AwakePostfix(ref Panel __instance)
    {
        ref var self = ref __instance;
        var obj = self.gameObject;

        var backgroundObject = obj.transform.Find("Mask/Background").gameObject;
        backgroundObject.transform.SetSiblingIndex(1);
        var img = backgroundObject.GetComponent<UnityEngine.UI.Image>();
        img.sprite = sprite;

        var currencyObject = obj.transform.Find("Mask/Currency").gameObject;
        currencyObject.transform.localPosition = new Vector3(284, 179, 0);

        cursePage = false;
    }

    static Sprite MakeSprite()
    {
        var assembly = typeof(FullCursePanelPatch).Assembly;
        var resource = assembly.GetManifestResourceStream("RerollCurses.UpgradeFrame.png");

        byte[] buf = new byte[resource.Length];
        resource.Read(buf, 0, (int)resource.Length);

        Texture2D texture = new(2, 2);
        texture.LoadImage(buf);
        texture.filterMode = FilterMode.Point;
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1.0f);
    }
}
