using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Abilities;
using Characters.Gear;
using Characters.Gear.Items;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace CustomItems.CustomAbilities;

[Serializable]
public class PandorasBoxAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<PandorasBoxAbility>
    {
        public Instance(Character owner, PandorasBoxAbility ability) : base(owner, ability)
        {
        }

        private static Rarity HigherRarity(Rarity rarity)
        {
            switch (rarity)
            {
                case Rarity.Common:
                    return Rarity.Rare;
                case Rarity.Rare:
                    return Rarity.Unique;
                default:
                    return Rarity.Legendary;
            }
        }

        private static IEnumerator Animation(Character owner, List<string> oldItems, List<string> newItems)
        {
            var spawner = Singleton<Service>.Instance.floatingTextSpawner;
            var titlePosition = new Vector3(owner.collider.bounds.center.x, owner.collider.bounds.max.y + 1.0f, 0);
            var position = new Vector3(owner.collider.bounds.center.x, owner.collider.bounds.max.y + 0.25f, 0);

            var title = spawner.Spawn("THE BOX HAS BEEN OPENED!", titlePosition);

            for (int i = 0; i < oldItems.Count; i++)
            {
                Singleton<Service>.Instance.levelManager.DropGold(500, 20);
                spawner.SpawnBuff("-" + oldItems[i], position, "#880088");
                yield return new WaitForSeconds(0.2f);
                spawner.SpawnBuff("+" + newItems[i], position, "#AAAA33");
                yield return new WaitForSeconds(0.5f);
            }

            title.FadeOut(0.5f);
        }

        public override void OnAttach()
        {
            var inventory = owner.playerComponents.inventory.item;
            var gearManager = Singleton<Service>.Instance.gearManager;
            var random = new System.Random(Data.GameData.Save.instance.randomSeed);

            List<string> oldItems = new();
            List<string> newItems = new();

            var levelManager = Singleton<Service>.Instance.levelManager;

            for (int i = 0; i < inventory.items.Count; i++)
            {
                try
                {
                    var item = inventory.items[i];

                    if (item == null)
                    {
                        continue;
                    }

                    oldItems.Add(item.displayName);

                    ItemReference itemRef = null;
                    if ((item.gearTag & Gear.Tag.Omen) == Gear.Tag.Omen)
                    {
                        itemRef = gearManager.GetOmenItems(random);
                    }
                    else
                    {
                        itemRef = gearManager.GetItemToTake(random, HigherRarity(item.rarity));
                    }

                    ItemRequest itemReq = itemRef.LoadAsync();
                    itemReq.WaitForCompletion();

                    Item newItem = levelManager.DropItem(itemReq, Vector3.zero);
                    item.ChangeOnInventory(newItem);

                    newItems.Add(newItem.displayName);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[CustomItems - Pandora's Box] Error at item index " + i);
                    Debug.LogWarning(e.Message);
                }
            }

            owner.StartCoroutine(Animation(owner, oldItems, newItems));
        }

        public override void OnDetach()
        {
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new PandorasBoxAbility();
    }
}
