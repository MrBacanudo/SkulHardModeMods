using System;
using Characters.Gear.Items;
using UnityEngine;

[Serializable]
public sealed class OmenKeywordRandomizer : KeywordRandomizer
{
    private new void Awake()
    {
        base.Awake();
        _item._gearTag = Characters.Gear.Gear.Tag.Omen;
        _item.keyword1 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.Omen;
    }
}
