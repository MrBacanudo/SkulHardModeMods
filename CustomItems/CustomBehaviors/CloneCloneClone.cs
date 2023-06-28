using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Gear.Items;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using CustomItems;
using UnityEngine;

[Serializable]
public sealed class CloneCloneClone : MonoBehaviour
{

    [SerializeField]
    private Item _item = null;

    private static Inscription.Key[] keywords = Enum.GetValues(typeof(Inscription.Key)).Cast<Inscription.Key>().ToArray();
    private static Dictionary<Inscription.Key, int> keywordIndexes = keywords.Select((value, index) => new { value, index })
                                                                             .ToDictionary(keyword => keyword.value, keyword => keyword.index);
    private HashSet<Inscription.Key> forbiddenKeywords = new(){
        Inscription.Key.None,
        Inscription.Key.SunAndMoon,
        // Inscription.Key.Masterpiece, // Nah, let people have some fun
        Inscription.Key.Omen,
        Inscription.Key.Sin,
    };

    private void Awake()
    {
        CustomItemsPatch.OnInventoryUpdate -= InventoryUpdate;
        CustomItemsPatch.OnInventoryUpdate += InventoryUpdate;

        if (_item == null)
        {
            return;
        }


        _item.bonusKeyword = new Item.BonusKeyword[keywords.Length];

        for (int i = 0; i < keywords.Length; i++)
        {
            _item.bonusKeyword[i] = new Item.BonusKeyword()
            {
                keyword = keywords[i],
                type = Item.BonusKeyword.Type.Single,
                count = 0,
            };
        }
    }

    private void OnDestroy()
    {
        CustomItemsPatch.OnInventoryUpdate -= InventoryUpdate;
    }

    private void InventoryUpdate(Inventory inventory)
    {
        if (_item == null || _item.bonusKeyword == null || _item.bonusKeyword.Length == 0)
        {
            return;
        }

        foreach (var bonus in _item.bonusKeyword)
        {
            bonus.count = 0;
        }

        foreach (var item in inventory.item.items)
        {
            if (item == null)
            {
                continue;
            }

            _item.bonusKeyword[keywordIndexes[item.keyword1]].count = 1;
            _item.bonusKeyword[keywordIndexes[item.keyword2]].count = 1;
        }

        foreach (Inscription.Key key in Inscription.keys)
        {
            if (inventory.synergy.inscriptions[key].bonusCount > 0)
            {
                _item.bonusKeyword[keywordIndexes[key]].count = 1;
            }
        }

        foreach (var bonus in _item.bonusKeyword)
        {
            if (forbiddenKeywords.Contains(bonus.keyword))
            {
                bonus.count = 0;
            }
            else
            {

                bonus.count *= 1; // TODO: make this multiplier a parameter
            }
        }
    }

}
