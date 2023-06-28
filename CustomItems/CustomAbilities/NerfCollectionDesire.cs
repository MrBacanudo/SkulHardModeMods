using System;
using System.Linq;
using Characters;
using Characters.Abilities;
using Characters.Abilities.CharacterStat;
using Characters.Player;
using UnityEngine;

namespace CustomItems.CustomAbilities;

[Serializable]
public class NerfCollectionDesire : Ability, ICloneable
{
    public class Instance : AbilityInstance<NerfCollectionDesire>
    {
        private Inventory _inventory;
        private bool _collectionPresent = false;
        public Instance(Character owner, NerfCollectionDesire ability) : base(owner, ability)
        {
            _inventory = owner.playerComponents.inventory;
        }

        public override void OnAttach()
        {
            RefreshCollectionDesire();
            _inventory.onUpdatedKeywordCounts += RefreshCollectionDesire;
            _inventory.upgrade.onChanged += RefreshCollectionDesire;
        }

        public override void OnDetach()
        {
            _inventory.upgrade.onChanged -= RefreshCollectionDesire;
            if (_collectionPresent)
            {
                _collectionPresent = false;
                AddToCollectionDesireRequirement(-ability._count);
            }
        }

        private void RefreshCollectionDesire()
        {
            AddToCollectionDesireRequirement(ability._count);
        }

        private void AddToCollectionDesireRequirement(int count)
        {
            foreach (var darkAbility in _inventory.upgrade.upgrades)
            {
                if (darkAbility == null)
                {
                    continue;
                }

                if (darkAbility.name == "CollectionDesire")
                {
                    if (!_collectionPresent)
                    {
                        _collectionPresent = true;
                        var component = darkAbility.GetComponentInChildren<StatBonusPerInscriptionStackComponent>();

                        if (component == null)
                        {
                            Debug.LogWarning("[Custom Items] Could not find a Collection Desire to nerf.");
                            return;
                        }

                        component._ability._inscriptionLevelForStackCounting += count;
                        _inventory.UpdateSynergy();
                    }
                    return;
                }
            }

            _collectionPresent = false;
        }
    }


    [SerializeField]
    internal int _count = 1;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new NerfCollectionDesire()
        {
            _count = _count,
        };
    }
}
