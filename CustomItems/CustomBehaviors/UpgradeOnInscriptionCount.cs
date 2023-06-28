using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Gear.Items;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using CustomItems;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

[Serializable]
public sealed class UpgradeOnInscriptionCount : MonoBehaviour
{

    [SerializeField]
    private Item _item = null;

    private void Awake()
    {
        Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.onUpdatedKeywordCounts += CheckUpdateCondition;
    }

    private void OnDestroy()
    {
        Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.onUpdatedKeywordCounts -= CheckUpdateCondition;
    }

    private void CheckUpdateCondition()
    {
        var inscriptions = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.inscriptions;
        var inscriptionNumber = inscriptions.Count((inscription) => inscription.count > 0);

        if (inscriptionNumber >= 14) // TODO: set a parameter for this, and allow gettin from the CustomItems definition
        {
            ItemReference itemRef;
            if (GearResource.instance.TryGetItemReferenceByName(_item.name + "_2", out itemRef))
            {

                ItemRequest request = itemRef.LoadAsync();
                request.WaitForCompletion();

                if (_item.state == Characters.Gear.Gear.State.Equipped)
                {
                    Item newItem = Singleton<Service>.Instance.levelManager.DropItem(request, Vector3.zero);
                    newItem.keyword1 = _item.keyword1;
                    newItem.keyword2 = _item.keyword2;
                    newItem._gearTag = _item._gearTag;
                    _item.ChangeOnInventory(newItem);

                    var titlePosition = new Vector3(_item.owner.collider.bounds.center.x, _item.owner.collider.bounds.max.y + 1.0f, 0);
                    Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff("IT'S INSANITY TIME!", titlePosition);
                }
            }
        }
    }

}
