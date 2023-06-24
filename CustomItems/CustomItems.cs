using System;
using System.Collections.Generic;
using Characters;
using Characters.Abilities;
using Characters.Abilities.CharacterStat;
using Characters.Gear.Items;
using Characters.Gear.Synergy.Inscriptions;
using CustomItems.CustomAbilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static Characters.CharacterStatus;
using static Characters.Damage;

namespace CustomItems;

public class CustomItems
{
    public static readonly CustomItemReference[] Items = InitializeItems();
    public static readonly Dictionary<string, string> Strings = InitializeStrings();

    public static readonly Masterpiece.EnhancementMap[] Masterpieces = InitializeMasterpieces();

    private static CustomItemReference[] InitializeItems()
    {
        List<CustomItemReference> items = new();
        {
            var item = new CustomItemReference();
            item.name = "DecidedlyPhysical";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = true;
            item.needUnlock = false;
            item.rarity = Rarity.Legendary;
            item.displayNameKey = "item/" + item.name + "/name";

            item.LoadSprites();

            item.itemName = "Orc King's Trusty Club";
            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 150%.\nYou cannot deal <color=#1787D8>Magic damage</color>.";
            item.itemLore = "The weapon of the strongest Orc. Once plated in gold, but he could never maintain it; his brute strength would shatter it.";

            item.prefabKeyword1 = Inscription.Key.Brave;
            item.prefabKeyword2 = Inscription.Key.Brave;

            item.stats = new Stat.Values(new Stat.Value[]{
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 1.5),
                new Stat.Value(Stat.Category.Percent, Stat.Kind.MagicAttackDamage, 0),
            });

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "PurelyMagical";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = true;
            item.needUnlock = false;
            item.rarity = Rarity.Legendary;
            item.displayNameKey = "item/" + item.name + "/name";

            item.LoadSprites();

            item.itemName = "Raven Lord's Medallion";
            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 150%.\nYou cannot deal <color=#F25D1C>Physical damage</color>.";
            item.itemLore = "Able to control his army of raven souls, the Raven Lord has never faced a physical confrontation for the rest of his life.";

            item.prefabKeyword1 = Inscription.Key.Wisdom;
            item.prefabKeyword2 = Inscription.Key.Wisdom;

            item.stats = new Stat.Values(new Stat.Value[]{
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 1.5),
                new Stat.Value(Stat.Category.Percent, Stat.Kind.PhysicalAttackDamage, 0),
            });

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "WisdomAndCourage";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = true;
            item.needUnlock = false;
            item.rarity = Rarity.Rare;
            item.displayNameKey = "item/" + item.name + "/name";

            item.LoadSprites();

            item.itemName = "Amulet of Duality";
            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> and <color=#F25D1C>Physical Attack</color> by 40%.";
            item.itemLore = "With a taste for knowledge, undecided warrior Haxa could never succumb to pure brutality.";

            item.prefabKeyword1 = Inscription.Key.Brave;
            item.prefabKeyword2 = Inscription.Key.Wisdom;

            item.stats = new Stat.Values(new Stat.Value[]{
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.4),
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.4),
            });

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "ElementalMess";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = true;
            item.needUnlock = false;
            item.rarity = Rarity.Legendary;
            item.displayNameKey = "item/" + item.name + "/name";

            item.LoadSprites();

            item.itemName = "Quindent of Sadism";
            item.itemDescription = "Attacks and skills have +10% chance to inflict any status effect: Poison, Wound, Stun, Freeze or Burn.\n"
                                + "(Freeze and Stun bonuses have an internal cooldown of 4 seconds.)";
            item.itemLore = "The Demonlord Beelz gets stronger by seeing all the pain and suffering on everyone around him.";

            item.prefabKeyword1 = Inscription.Key.Execution;
            item.prefabKeyword2 = Inscription.Key.Misfortune;

            item.stats = new Stat.Values(new Stat.Value[] { });

            item.abilities = new Ability[5];

            for (int i = 0; i < 5; i++)
            {
                var ability = new ApplyStatusOnGaveDamage();
                var status = (Kind)i;
                ability._cooldownTime = (status == Kind.Freeze || status == Kind.Stun) ? 4.0f : 0.1f;
                ability._chance = 10;
                ability._attackTypes = new();
                ability._attackTypes[MotionType.Basic] = true;
                ability._attackTypes[MotionType.Skill] = true;

                ability._types = new();
                ability._types[AttackType.Melee] = true;
                ability._types[AttackType.Ranged] = true;
                ability._types[AttackType.Projectile] = true;

                ability._status = new CharacterStatus.ApplyInfo(status);

                item.abilities[i] = ability;
            }

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "FireAndIce";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = true;
            item.needUnlock = false;
            item.rarity = Rarity.Rare;
            item.displayNameKey = "item/" + item.name + "/name";

            item.LoadSprites();

            item.itemName = "Elementalist's Staff";
            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 25%.\n"
                                + "Skills have +5% chance to inflict Freeze or Burn.\n"
                                + "(Freeze bonus has a cooldown of 2 seconds.)";
            item.itemLore = "Only a true master of the elements could learn how to set ice on fire.";

            item.prefabKeyword1 = Inscription.Key.Arson;
            item.prefabKeyword2 = Inscription.Key.AbsoluteZero;

            item.stats = new Stat.Values(new Stat.Value[] {
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.25),
            });

            Kind[] statuses = { Kind.Freeze, Kind.Burn };

            item.abilities = new Ability[statuses.Length];

            for (int i = 0; i < statuses.Length; i++)
            {
                var ability = new ApplyStatusOnGaveDamage();
                var status = statuses[i];
                ability._cooldownTime = (status == Kind.Freeze) ? 2.0f : 0.1f;
                ability._chance = 10;
                ability._attackTypes = new();
                ability._attackTypes[MotionType.Skill] = true;

                ability._types = new();
                ability._types[AttackType.Melee] = true;
                ability._types[AttackType.Ranged] = true;
                ability._types[AttackType.Projectile] = true;

                ability._status = new CharacterStatus.ApplyInfo(status);

                item.abilities[i] = ability;
            }

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "PoisonAndBleed";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = true;
            item.needUnlock = false;
            item.rarity = Rarity.Rare;
            item.displayNameKey = "item/" + item.name + "/name";

            item.LoadSprites();

            item.itemName = "Hydra's Fangs";
            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 25%.\n"
                                + "Skills have +5% chance to inflict Poison or Wound.";
            item.itemLore = "Hydras are known for their venom, but victims of their bite would never stop bleeding.";

            item.prefabKeyword1 = Inscription.Key.Poisoning;
            item.prefabKeyword2 = Inscription.Key.ExcessiveBleeding;

            item.stats = new Stat.Values(new Stat.Value[] {
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.25),
            });

            Kind[] statuses = { Kind.Wound, Kind.Poison };

            item.abilities = new Ability[statuses.Length];

            for (int i = 0; i < statuses.Length; i++)
            {
                var ability = new ApplyStatusOnGaveDamage();
                var status = statuses[i];
                ability._cooldownTime = 0.1f;
                ability._chance = 10;
                ability._attackTypes = new();
                ability._attackTypes[MotionType.Skill] = true;

                ability._types = new();
                ability._types[AttackType.Melee] = true;
                ability._types[AttackType.Ranged] = true;
                ability._types[AttackType.Projectile] = true;

                ability._status = new CharacterStatus.ApplyInfo(status);

                item.abilities[i] = ability;
            }

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "CarleonHeritage";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = true;
            item.needUnlock = false;
            item.rarity = Rarity.Common;
            item.displayNameKey = "item/" + item.name + "/name";

            item.LoadSprites();

            item.itemName = "Carleon's Flag";
            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15% for each Carleon item you own.";
            item.itemLore = "The symbol of Carleon's people brought strength during times of need.";

            item.prefabKeyword1 = Inscription.Key.Spoils;
            item.prefabKeyword2 = Inscription.Key.Heritage;

            item.gearTag = Characters.Gear.Gear.Tag.Carleon;

            item.stats = new Stat.Values(new Stat.Value[] { });

            StatBonusPerGearTag ability = new();

            ability._tag = Characters.Gear.Gear.Tag.Carleon;

            ability._statPerGearTag = new Stat.Values(new Stat.Value[] {
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.15),
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.15),
            });

            item.abilities = new Ability[] { ability };

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "BoneOfJumps";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = true;
            item.needUnlock = false;
            item.rarity = Rarity.Rare;
            item.displayNameKey = "item/" + item.name + "/name";

            item.LoadSprites();

            item.itemName = "Wing Bone";
            item.itemDescription = "Grants 1 extra jump (Except for Gargoyle).\n"
                                 + "Decreases gravity by 50%.\n"
                                 + "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 25% while in midair.";
            item.itemLore = "???";

            item.prefabKeyword1 = Inscription.Key.Bone;
            item.prefabKeyword2 = Inscription.Key.Soar;

            item.stats = new Stat.Values(new Stat.Value[] { });

            AddAirJumpCount jumpAbility = new();

            StatBonusByAirTime bonus = new();

            bonus._timeToMaxStat = 0.01f;
            bonus._remainTimeOnGround = 0.0f;
            bonus._maxStat = new Stat.Values(new Stat.Value[] {
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.25),
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.25),
            });

            GravityScale gravityAbility = new() { amount = 0.5f };

            item.abilities = new Ability[] { bonus, jumpAbility, gravityAbility };

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "TalariaOfMercury";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = true;
            item.needUnlock = false;
            item.rarity = Rarity.Unique;
            item.displayNameKey = "item/" + item.name + "/name";

            item.LoadSprites();

            item.itemName = "Talaria of Mercury";
            item.itemDescription = "Grants 1 extra dash.\nIncreases movement speed by 50%.";
            item.itemLore = "Mortals could never achieve Hermes's speed. The key to his swiftness was his winged sandals.";

            item.prefabKeyword1 = Inscription.Key.Chase;
            item.prefabKeyword2 = Inscription.Key.Rapidity;

            item.stats = new Stat.Values(new Stat.Value[] {
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MovementSpeed, 0.5),
            });

            item.abilities = new Ability[] {
                new ExtraDash(),
            };

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "TheTreasureOmen";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = false;
            item.needUnlock = false;
            item.rarity = Rarity.Unique;
            item.displayNameKey = "item/" + item.name + "/name";

            item.gearTag = Characters.Gear.Gear.Tag.Omen;

            item.LoadSprites();

            item.itemName = "Omen: Curse of Greed";
            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 1% for each 100 gold you have.";
            item.itemLore = "...";

            item.prefabKeyword1 = Inscription.Key.Omen;
            item.prefabKeyword2 = Inscription.Key.Treasure;

            item.stats = new Stat.Values(new Stat.Value[] { });

            item.abilities = new Ability[] {
                new StatBonusByGoldAmount(){
                    _statsPerStack = new Stat.Values(new Stat.Value[] {
                        new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.01),
                        new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.01),
                    }),
                    _goldPerStack = 100
                }
            };

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "PandorasBox";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = true;
            item.needUnlock = false;
            item.rarity = Rarity.Legendary;
            item.displayNameKey = "item/" + item.name + "/name";

            item.LoadSprites();

            item.itemName = "Pandora's Box";
            item.itemDescription = "Upon pickup, replace each item with a random item of higher rarity.\nYou gain 500 gold per item destroyed.";
            item.itemLore = "\"This is my own special gift to you. Don't ever open it.\"";

            item.prefabKeyword1 = Inscription.Key.Heirloom;
            item.prefabKeyword2 = Inscription.Key.Treasure;

            item.stats = new Stat.Values(new Stat.Value[] { });

            item.abilities = new Ability[] {
                new PandorasBoxAbility(),
            };

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "QuintDamageBuff";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = true;
            item.needUnlock = false;
            item.rarity = Rarity.Rare;
            item.displayNameKey = "item/" + item.name + "/name";

            item.LoadSprites();

            item.itemName = "Claws of Raptor";
            item.itemDescription = "Amplifies Quintessence damage by 30%.\nIncreases crit damage by 15%.";
            item.itemLore = "No one has ever seen the monster that possessed such powerful claws. At least no one who survived.";

            item.prefabKeyword1 = Inscription.Key.Heritage;
            item.prefabKeyword2 = Inscription.Key.Misfortune;

            item.stats = new Stat.Values(new Stat.Value[]{
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.15),
            });

            ModifyDamage quintDamage = new();

            quintDamage._attackTypes = new();
            quintDamage._attackTypes[Damage.MotionType.Quintessence] = true;

            quintDamage._damageTypes = new(new[] { true, true, true, true, true });

            quintDamage._damagePercent = 1.3f;

            item.abilities = new Ability[] {
                quintDamage
            };

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "MisfortuneBrawl";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = true;
            item.needUnlock = false;
            item.rarity = Rarity.Common;
            item.displayNameKey = "item/" + item.name + "/name";

            item.LoadSprites();

            item.itemName = "[Misfortune Brawl Item]";
            item.itemDescription = "Increases crit damage by 10%.\nIncreases crit rate by 5%.";
            item.itemLore = "???";

            item.prefabKeyword1 = Inscription.Key.Misfortune;
            item.prefabKeyword2 = Inscription.Key.Brawl;

            item.stats = new Stat.Values(new Stat.Value[]{
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.10),
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, 0.05),
            });

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "CommonMasterpiece";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = true;
            item.needUnlock = false;
            item.rarity = Rarity.Common;
            item.displayNameKey = "item/" + item.name + "/name";

            item.LoadSprites();

            item.itemName = "[Common Masterpiece Item]";
            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 20%.";
            item.itemLore = "???";

            item.prefabKeyword1 = Inscription.Key.Masterpiece;
            item.prefabKeyword2 = Inscription.Key.Execution;

            item.stats = new Stat.Values(new Stat.Value[]{
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.20)
            });

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "CommonMasterpiece_2";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = false;
            item.needUnlock = false;
            item.rarity = Rarity.Common;
            item.displayNameKey = "item/" + item.name + "/name";

            item.LoadSprites();

            item.itemName = "[Common Masterpiece Item, Evolved]";
            item.itemDescription = "Amplifies <color=#F25D1C>Physical Attack</color> by 20%.";
            item.itemLore = "???";

            item.prefabKeyword1 = Inscription.Key.Masterpiece;
            item.prefabKeyword2 = Inscription.Key.Execution;

            item.forbiddenDrops = new[] { "CommonMasterpiece" };

            item.stats = new Stat.Values(new Stat.Value[]{
                new Stat.Value(Stat.Category.Percent, Stat.Kind.PhysicalAttackDamage, 1.20)
            });

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "TheManatechOmen";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = false;
            item.needUnlock = false;
            item.rarity = Rarity.Unique;
            item.displayNameKey = "item/" + item.name + "/name";

            item.gearTag = Characters.Gear.Gear.Tag.Omen;

            item.LoadSprites();

            item.itemName = "Omen: Dark Tech Marvel";
            item.itemDescription = "Picking up a Manatech part amplifies Skill Damage by 5% for 8 seconds (Up to 50%)";
            item.itemLore = "???";

            item.prefabKeyword1 = Inscription.Key.Omen;
            item.prefabKeyword2 = Inscription.Key.Manatech;

            item.stats = new Stat.Values(new Stat.Value[] { });

            StatBonusPerManatechPart bonus = new();

            bonus._timeout = 8.0f;
            bonus._maxStack = 10;
            bonus._statPerStack = new Stat.Values(new Stat.Value[] {
                new Stat.Value(Stat.Category.Percent, Stat.Kind.SkillAttackDamage, 0.05),
            });

            item.abilities = new Ability[] { bonus };

            items.Add(item);
        }


        {
            var item = new CustomItemReference();
            item.name = "AdventurerKiller";
            item.guid = "custom_item://" + item.name;
            item.path = "Assets/Gear/Items/BasicCarleonSword.prefab";
            item.obtainable = true;
            item.needUnlock = false;
            item.rarity = Rarity.Unique;
            item.displayNameKey = "item/" + item.name + "/name";

            item.LoadSprites();

            item.itemName = "...";
            item.itemDescription = "Upon killing an adventurer, this item becomes that adventurer's legendary item.";
            item.itemLore = "...";

            item.prefabKeyword1 = Inscription.Key.Duel;
            item.prefabKeyword2 = Inscription.Key.Execution;

            item.stats = new Stat.Values(new Stat.Value[] { });

            item.abilities = new Ability[] {
                new AdventurerWeaponSteal(){
                    baseItem = item.name
                }
            };

            items.Add(item);
        }


        return items.ToArray();
    }

    private static Dictionary<string, string> InitializeStrings()
    {
        Dictionary<string, string> strings = new(Items.Length * 8);

        foreach (var item in Items)
        {
            strings.Add("item/" + item.name + "/name", item.itemName);
            strings.Add("item/" + item.name + "/desc", item.itemDescription);
            strings.Add("item/" + item.name + "/flavor", item.itemLore);
        }

        return strings;
    }

    private static Masterpiece.EnhancementMap[] InitializeMasterpieces()
    {
        List<Masterpiece.EnhancementMap> maps = new();

        // TODO: something not so ugly and O(N²)
        foreach (var item in Items)
        {
            if (item.prefabKeyword1 != Inscription.Key.Masterpiece && item.prefabKeyword2 != Inscription.Key.Masterpiece)
            {
                continue;
            }

            foreach (var item2 in Items)
            {
                if (item2.name.Equals(item.name + "_2"))
                {
                    maps.Add(new()
                    {
                        _from = new AssetReference(item.guid),
                        _to = new AssetReference(item2.guid),
                    });
                }
            }
        }

        return maps.ToArray();
    }
}
