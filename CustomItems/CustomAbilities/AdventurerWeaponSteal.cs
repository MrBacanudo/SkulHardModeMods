using System;
using System.Collections.Generic;
using Characters;
using Characters.Abilities;
using Characters.Gear.Items;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace CustomItems.CustomAbilities;

[Serializable]
public class AdventurerWeaponSteal : Ability, ICloneable
{
    public class Instance : AbilityInstance<AdventurerWeaponSteal>
    {
        private static Dictionary<string, string> AdventurerItems = new(){
            {"AdventurerArcher", "TheChosenHuntersArrow"},
            {"AdventurerCleric", "TheChosenClericsBible"},
            {"AdventurerHero", "TheChosenHerosCirclet"},
            {"AdventurerMagician", "TheChosenMagesBadge"},
            {"AdventurerThief", "TheChosenThiefsTwinSwords"},
            {"AdventurerWarrior", "TheChosenWarriorsArmor"},

            {"SupportingArcher", "TheChosenHuntersArrow"},
            {"SupportingCleric", "TheChosenClericsBible"},
            {"SupportingHero", "TheChosenHerosCirclet"},
            {"SupportingMagician", "TheChosenMagesBadge"},
            {"SupportingThief", "TheChosenThiefsTwinSwords"},
            {"SupportingWarrior", "TheChosenWarriorsArmor"},

            {"Veteran Archer", "TheChosenHuntersArrow"},
            {"Veteran Cleric", "TheChosenClericsBible"},
            {"Veteran Hero", "TheChosenHerosCirclet"},
            {"Veteran Magician", "TheChosenMagesBadge"},
            {"Veteran Thief", "TheChosenThiefsTwinSwords"},
            {"Veteran Warrior", "TheChosenWarriorsArmor"},
        };
        bool dropped = false;

        public Instance(Character owner, AdventurerWeaponSteal ability) : base(owner, ability)
        {
        }

        private void OnKill(ITarget target, ref Damage damage)
        {
            Character character = target.character;

            if (dropped || character.type != Character.Type.Adventurer)
            {
                return;
            }

            string name = character.name;
            string itemName = null;

            foreach (var key in AdventurerItems.Keys)
            {
                if (name.StartsWith(key))
                {
                    itemName = AdventurerItems[key];
                    break;
                }
            }

            if (itemName == null)
            {
                Debug.LogWarning("[CustomItems - Essence Extractor] Adventurer '" + name + "' not found.");
                return;
            }

            var inventory = owner.playerComponents.inventory.item;

            for (int i = 0; i < inventory.items.Count; i++)
            {
                try
                {
                    var item = inventory.items[i];

                    if (item == null)
                    {
                        continue;
                    }

                    if (item.name == ability.baseItem)
                    {
                        ItemReference itemRef;
                        if (!GearResource.instance.TryGetItemReferenceByName(itemName, out itemRef))
                        {
                            Debug.LogWarning("[CustomItems - Essence Extractor] Could not find item " + itemName);
                        }

                        ItemRequest itemReq = itemRef.LoadAsync();
                        itemReq.WaitForCompletion();

                        item.RemoveOnInventory();
                        Item newItem = Singleton<Service>.Instance.levelManager.DropItem(itemReq, character.collider.bounds.center);

                        var titlePosition = new Vector3(character.collider.bounds.center.x, character.collider.bounds.max.y + 1.0f, 0);
                        Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff("YOUR SOUL IS MINE", titlePosition);

                        dropped = true;

                        return;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[CustomItems - Essence Extractor] Error at item index " + i);
                    Debug.LogWarning(e.Message);
                }
            }

        }

        public override void OnAttach()
        {
            owner.onKilled += OnKill;
        }

        public override void OnDetach()
        {
            owner.onKilled -= OnKill;
        }
    }

    [SerializeField]
    internal string baseItem;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new AdventurerWeaponSteal()
        {
            baseItem = baseItem
        };
    }
}
