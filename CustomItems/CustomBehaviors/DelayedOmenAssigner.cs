using System;
using Characters.Gear.Items;
using UnityEngine;

[Serializable]
public sealed class DelayedOmenAssigner : MonoBehaviour
{
    [SerializeField]
    private Item _item = null;
    private void Awake()
    {
        _item._gearTag = Characters.Gear.Gear.Tag.Omen;
        _item.keyword1 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.Omen;
    }
}
