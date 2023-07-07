using System;
using System.Collections.Generic;
using System.Linq;
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
    public static readonly List<CustomItemReference> Items = InitializeItems();

    private static List<CustomItemReference> InitializeItems()
    {
        List<CustomItemReference> items = new();
        {
            var item = new CustomItemReference();
            item.name = "DecidedlyPhysical";
            item.rarity = Rarity.Legendary;

            item.itemName = "Orc King's Trusty Club";
            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 100%.\nYou cannot deal <color=#1787D8>Magic damage</color>.";
            item.itemLore = "The weapon of the strongest Orc. Once plated in gold, but he could never maintain it; his brute strength would shatter it.";

            item.prefabKeyword1 = Inscription.Key.Brave;
            item.prefabKeyword2 = Inscription.Key.Brave;

            item.stats = new Stat.Values(new Stat.Value[] { });

            StatBonus bonus = new();

            bonus._stat = new Stat.Values(new Stat.Value[]{
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 1.0),
                new Stat.Value(Stat.Category.Percent, Stat.Kind.MagicAttackDamage, 0),
            });

            item.abilities = new Ability[] { bonus };


            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "PurelyMagical";
            item.rarity = Rarity.Legendary;

            item.itemName = "Raven Lord's Medallion";
            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 100%.\nYou cannot deal <color=#F25D1C>Physical damage</color>.";
            item.itemLore = "Able to control his army of raven souls, the Raven Lord has never faced a physical confrontation for the rest of his life.";

            item.prefabKeyword1 = Inscription.Key.Wisdom;
            item.prefabKeyword2 = Inscription.Key.Wisdom;

            item.stats = new Stat.Values(new Stat.Value[] { });

            StatBonus bonus = new();

            bonus._stat = new Stat.Values(new Stat.Value[]{
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 1.0),
                new Stat.Value(Stat.Category.Percent, Stat.Kind.PhysicalAttackDamage, 0),
            });

            item.abilities = new Ability[] { bonus };

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "WisdomAndCourage";
            item.rarity = Rarity.Rare;

            item.itemName = "Amulet of Duality";
            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 40%.";
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
            item.rarity = Rarity.Legendary;

            item.itemName = "Quindent of Sadism";
            item.itemDescription = "Attacks and skills have +10% chance to inflict any status effect: Poison, Wound, Stun, Freeze or Burn.\n"
                                + "(Freeze and Stun bonuses have an internal cooldown of 4 seconds.)";
            item.itemLore = "The Demonlord Beelz becomes stronger by inflicting pain and suffering on everyone around him.";

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
            item.rarity = Rarity.Rare;

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
            item.rarity = Rarity.Rare;

            item.itemName = "Poisoned Rope Dart";
            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 25%.\n"
                                + "Skills have +5% chance to inflict Poison or Wound.";
            item.itemLore = "In skilled hands, this kunai brings death as swiftly as a Scorpion's sting.";

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
            item.rarity = Rarity.Common;

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
            item.rarity = Rarity.Common;

            item.itemName = "Bone Wing";
            item.itemDescription = "Grants 1 extra jump and decreases gravity by 50%.\n"
                                 + "(Gargoyle: reduces falling speed while attacking or using skills instead)\n"
                                 + "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 25% while in midair.";
            item.itemLore = "Float like a butterfly, sting like a bee!";

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
            item.rarity = Rarity.Unique;

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
            item.rarity = Rarity.Unique;

            item.gearTag = Characters.Gear.Gear.Tag.Omen;
            item.obtainable = false; // Omens should be unobtainable

            item.itemName = "Omen: Root of All Evil";
            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 1% for each 100 gold you have.\n"
                                 + "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 20% for each Treasure inscription you have.";
            item.itemLore = "Wealth is the true root of all power, for greed knows no bounds.";

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
                },
                new StatBonusByInscriptionCount(){
                    _keys = new[]{Inscription.Key.Treasure},
                    _statPerStack = new Stat.Values(new Stat.Value[] {
                        new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.2),
                        new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.2),
                    })
                },
            };

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "PandorasBox";
            item.rarity = Rarity.Legendary;

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
            item.rarity = Rarity.Rare;

            item.itemName = "Claws of Raptor";
            item.itemDescription = "Amplifies Quintessence damage by 30%.\nIncreases crit damage by 15%.";
            item.itemLore = "No one has ever seen the monster that possessed such powerful claws and survived to tell the story.";

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
            item.rarity = Rarity.Common;

            item.itemName = "Spiked Brass Knuckles";
            item.itemDescription = "Increases crit damage by 10%.\nIncreases crit rate by 5%.";
            item.itemLore = "There is no such thing as a fair fight. Only a fight you win.";

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
            item.rarity = Rarity.Common;

            item.itemName = "Ornamental Axe";
            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 20%.";
            item.itemLore = "During times of distress, anything can be used as a weapon.";

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
            item.obtainable = false; // Evolutions should not be obtainable by default
            item.rarity = Rarity.Common;

            item.itemName = "Honorary Battle-Axe";
            item.itemDescription = "Amplifies <color=#F25D1C>Physical Attack</color> by 20%.";
            item.itemLore = "A weapon reforged by the fire of its warrior's soul.";

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
            item.rarity = Rarity.Unique;

            item.gearTag = Characters.Gear.Gear.Tag.Omen;
            item.obtainable = false; // Omens should be unobtainable

            item.itemName = "Omen: Dark Tech Marvel";
            item.itemDescription = "Picking up a Manatech part amplifies Skill Damage by 5% for 5 seconds (Up to 50%)";
            item.itemLore = "A paradoxical machine that never seems to stop running.";

            item.prefabKeyword1 = Inscription.Key.Omen;
            item.prefabKeyword2 = Inscription.Key.Manatech;

            item.stats = new Stat.Values(new Stat.Value[] { });

            StatBonusPerManatechPart bonus = new();

            bonus._timeout = 5.0f;
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
            item.rarity = Rarity.Unique;

            item.itemName = "Soul Extractor";
            item.itemDescription = "Upon killing an adventurer, this item disappears and that adventurer's Legendary item is dropped.";
            item.itemLore = "One's soul is the source of their most desirable posessions.";

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

        {
            var item = new CustomItemReference();
            item.name = "SymbolOfConfidence";
            item.rarity = Rarity.Unique;

            item.itemName = "Symbol of Confidence";
            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by up to 100%, equal to your current HP %.";
            item.itemLore = "The blistering confidence brought by a strong defense makes the most powerful warriors.";

            item.prefabKeyword1 = Inscription.Key.Antique;
            item.prefabKeyword2 = Inscription.Key.Fortress;

            item.stats = new Stat.Values(new Stat.Value[] { });

            StatBonusPerHPPercent bonus = new();

            bonus._maxStat = new Stat.Values(new Stat.Value[] {
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 1.0),
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 1.0),
            });

            item.abilities = new Ability[] { bonus };

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "OmenClone";
            item.rarity = Rarity.Unique;

            item.gearTag = Characters.Gear.Gear.Tag.Omen;
            item.obtainable = false; // Omens should be unobtainable

            item.itemName = "Omen: Cryptic Collectible";
            item.itemDescription = "This item gais one random inscription.\n"
                                 + "When you have 14 or more different inscriptions, transform this item.";
            item.itemLore = "The heart of a true collector desires what it desires. Even when it doesn't know what lies inside.";

            item.prefabKeyword1 = Inscription.Key.Omen;
            item.prefabKeyword2 = Inscription.Key.None;

            item.stats = new Stat.Values(new Stat.Value[] { });
            item.abilities = new Ability[] {
                new InscriptionCountAbility(),
            };

            item.extraComponents = new[] {
                typeof(OmenKeywordRandomizer),
                typeof(UpgradeOnInscriptionCount),
            };

            items.Add(item);
        }

        {
            var item = new CustomItemReference();
            item.name = "OmenClone_2";
            item.obtainable = false; // Omens are not obtainable, and neither should be evolutions.
            item.rarity = Rarity.Unique;

            item.itemName = "Omen: Idol of Insanity";
            item.itemDescription = "This item gais one random inscription.\n"
                                 + "Increases the number of each inscription you have by 1.\n"
                                 + "Increases Collection Desire's inscription requirement by 1.";
            item.itemLore = "I DON'T DESIRE ANYTHING ANYMORE! I HAVE EVERYTHING I'VE EVER WANTED!";

            // Omens are unobtainable, so they are found by their tag.
            // So we don't put the Omen tag or inscription here, and leave for the evolution process to copy them.
            item.prefabKeyword1 = Inscription.Key.None;
            item.prefabKeyword2 = Inscription.Key.None;

            item.stats = new Stat.Values(new Stat.Value[] { });

            item.abilities = new Ability[] {
                new NerfCollectionDesire(){
                    _count = 1
                }
            };

            item.extraComponents = new[] {
                typeof(OmenKeywordRandomizer), // Allows dropping the item with DevMenu or Machine
                typeof(CloneCloneClone),
            };

            item.forbiddenDrops = new[] { "OmenClone" };

            items.Add(item);
        }

        return items;
    }

    internal static void LoadSprites()
    {
        Items.ForEach(item => item.LoadSprites());
    }

    internal static Dictionary<string, string> MakeStringDictionary()
    {
        Dictionary<string, string> strings = new(Items.Count * 8);

        foreach (var item in Items)
        {
            strings.Add("item/" + item.name + "/name", item.itemName);
            strings.Add("item/" + item.name + "/desc", item.itemDescription);
            strings.Add("item/" + item.name + "/flavor", item.itemLore);
        }

        return strings;
    }

    internal static List<Masterpiece.EnhancementMap> ListMasterpieces()
    {
        var masterpieces = Items.Where(i => (i.prefabKeyword1 == Inscription.Key.Masterpiece) || (i.prefabKeyword2 == Inscription.Key.Masterpiece))
                                .ToDictionary(i => i.name);

        return masterpieces.Where(item => masterpieces.ContainsKey(item.Key + "_2"))
                           .Select(item => new Masterpiece.EnhancementMap()
                           {
                               _from = new AssetReference(item.Value.guid),
                               _to = new AssetReference(masterpieces[item.Key + "_2"].guid),
                           })
                           .ToList();
    }
}
