using System;
using Characters.Gear.Items;
using UnityEngine;

[Serializable]
public sealed class OmenKeywordRandomizer : KeywordRandomizer
{
    private new void Awake()
    {
        base.Awake();

        _item.keyword1 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.Omen;
    }
}
