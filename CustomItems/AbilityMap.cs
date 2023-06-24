using System;
using System.Collections.Generic;
using Characters.Abilities;
using Characters.Abilities.CharacterStat;
using CustomItems.CustomAbilities;

namespace CustomItems;

internal class AbilityMap
{

    internal static Dictionary<Type, Type> Map = new() {
        {typeof(ApplyStatusOnGaveDamage), typeof(ApplyStatusOnGaveDamageComponent)},
        {typeof(StatBonusPerGearTag), typeof(StatBonusPerGearTagComponent)},
        {typeof(StatBonusByAirTime), typeof(StatBonusByAirTimeComponent)},
        {typeof(AddAirJumpCount), typeof(AddAirJumpCountComponent)},
        {typeof(ExtraDash), typeof(ExtraDashComponent)},
        {typeof(StatBonusByGoldAmount), typeof(StatBonusByGoldAmountComponent)},
        {typeof(PandorasBoxAbility), typeof(PandorasBoxAbilityComponent)},
        {typeof(ModifyDamage), typeof(ModifyDamageComponent)},
        {typeof(GravityScale), typeof(GravityScaleComponent)},
        {typeof(StatBonusPerManatechPart), typeof(StatBonusPerManatechPartComponent)},
        {typeof(AdventurerWeaponSteal), typeof(AdventurerWeaponStealComponent)},
    };
}
