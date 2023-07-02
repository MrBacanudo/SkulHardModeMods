using System;
using System.Linq;
using System.Reflection;
using Characters;
using Characters.Abilities;
using Characters.Abilities.CharacterStat;
using Characters.Gear.Items;
using GameResources;
using HarmonyLib;
using UnityEngine;

namespace CustomItems;

[Serializable]
public class CustomItemReference : ItemReference
{
    private string _originalName;
    public new string name
    {
        get { return base.name; }
        set
        {
            _originalName = value;
            base.name = "Custom-" + _originalName;
            guid = "custom_item://" + value;
            displayNameKey = "item/" + base.name + "/name";
        }
    }

    public string itemName;
    public string itemDescription;
    public string itemLore;
    public Stat.Values stats;

    public Ability[] abilities;

    public Type[] extraComponents;

    public string[] forbiddenDrops = new string[0];
    public Sprite miniIcon;
    private Item item = null;

    private static GameObject rootObj = null;

    public CustomItemReference()
    {
        obtainable = true;
        needUnlock = false;
        path = "Assets/Gear/Items/BasicCarleonSword.prefab";
    }

    public Item GetItem(Item baseItem)
    {
        if (item == null)
        {
            if (rootObj == null)
            {
                rootObj = new GameObject("CustomItems");
                rootObj.SetActive(false);
                UnityEngine.Object.DontDestroyOnLoad(rootObj);
            }

            item = UnityEngine.Object.Instantiate<Item>(baseItem, rootObj.transform);
            UnityEngine.Object.DontDestroyOnLoad(item);

            item.gameObject.name = name;

            item.keyword1 = prefabKeyword1;
            item.keyword2 = prefabKeyword2;
            item._stat = stats;
            item._rarity = rarity;
            item._gearTag = gearTag;

            item._groupItemKeys = forbiddenDrops;

            LoadSprites();
            if (icon != null)
            {
                item.dropped.spriteRenderer.sprite = icon;
            }

            if (abilities != null && abilities.Length != 0)
            {
                GameObject attacherComponent = new("Ability Attacher");
                attacherComponent.transform.parent = item.gameObject.transform;

                var attacher = item._abilityAttacher = new();
                attacher._container = attacherComponent;

                attacher._components = new AbilityAttacher[abilities.Length];

                abilities[0]._defaultIcon = miniIcon;

                for (int i = 0; i < abilities.Length; i++)
                {
                    GameObject attacherObj = new GameObject("[" + i + "]", new Type[] { typeof(AlwaysAbilityAttacher) });
                    attacherObj.transform.parent = attacherComponent.transform;
                    AlwaysAbilityAttacher aa = attacherObj.GetComponent<AlwaysAbilityAttacher>();

                    aa._abilityComponent = CreateAbilityObject(attacherObj, abilities[i]);

                    attacher._components[i] = aa;
                }
            }

            if (extraComponents != null && extraComponents.Length != 0)
            {
                GameObject extraBehaviors = new GameObject("Extra Behaviors");
                extraBehaviors.transform.parent = item.gameObject.transform;

                foreach (var componentType in extraComponents)
                {
                    Component component = extraBehaviors.AddComponent(componentType);
                    FieldInfo field = AccessTools.Field(componentType, "_item");
                    if (field != null)
                    {
                        field.SetValue(component, item);
                    }
                }
            }

            //item.gameObject.SetActive(false);
        }

        // TODO: Clone, like we did with the wheel.
        return item;
    }

    private static AbilityComponent CreateAbilityObject(GameObject parent, Ability ability)
    {

        var abilityType = ability.GetType();
        var assembly = abilityType.Assembly;
        var componentName = abilityType.Name + "Component";
        var possibleTypes = assembly.GetTypes()
              .Where(type => String.Equals(type.Namespace, abilityType.Namespace, StringComparison.Ordinal)
                          && String.Equals(type.Name, componentName, StringComparison.Ordinal))
              .ToArray();

        if (possibleTypes.Length == 1)
        {
            var componentType = possibleTypes[0];
            GameObject abilityObj = new GameObject("Ability", new Type[] { componentType });
            abilityObj.transform.parent = parent.transform;

            FieldInfo field = AccessTools.Field(componentType, "_ability");
            Component component = abilityObj.GetComponent(componentType);
            field.SetValue(component, ability);

            return (AbilityComponent)component;
        }

        throw new NotImplementedException("Ability Component Type " + componentName + " not found.");
    }

    public void LoadSprites()
    {
        GetIcon();
        GetThumbnail();
        GetMiniIcon();
    }

    public Sprite GetIcon()
    {
        if (icon == null)
        {
            icon = LoadSprite("Icon", 32);
        }
        return icon;
    }
    public Sprite GetThumbnail()
    {
        if (thumbnail == null)
        {
            thumbnail = LoadSprite("Thumbnail", 32);

            if (thumbnail != null)
            {
                thumbnail.name = name;
            }
        }
        return thumbnail;
    }
    public Sprite GetMiniIcon()
    {
        if (miniIcon == null)
        {
            miniIcon = LoadSprite("AbilityIcon", 36);

            if (miniIcon != null)
            {
                miniIcon.name = name;
            }
        }
        return miniIcon;
    }

    private Sprite LoadSprite(string type, float pixelsPerUnit)
    {
        Sprite sprite = null;

        try
        {
            var assembly = typeof(CustomItemReference).Assembly;
            var resource = assembly.GetManifestResourceStream("CustomItems.Sprites." + _originalName + "." + type + ".png");

            byte[] buf = new byte[resource.Length];
            resource.Read(buf, 0, (int)resource.Length);

            Texture2D texture = new(2, 2);
            texture.LoadImage(buf);
            texture.filterMode = FilterMode.Point;

            var side = Math.Max(texture.width, texture.height);

            sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
        }
        catch (Exception)
        {
            Debug.LogWarning("[CustomItems] Could not load the " + type + " sprite for " + name);
        }

        return sprite;
    }
}
