using System;
using System.Linq;
using Characters;
using Characters.Abilities;
using Characters.Abilities.CharacterStat;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using UnityEngine;

namespace CustomItems.CustomAbilities;

[Serializable]
public class ReduceBoneInscriptionRequirement : Ability, ICloneable
{
    public class Instance : AbilityInstance<ReduceBoneInscriptionRequirement>
    {
        private bool _bonePresent = false;
        public Instance(Character owner, ReduceBoneInscriptionRequirement ability) : base(owner, ability)
        {

        }

        public override void OnAttach()
        {
            InventoryUpdate(owner.playerComponents.inventory);
            CustomItemsPatch.AfterInventoryUpdate -= InventoryUpdate;
            CustomItemsPatch.AfterInventoryUpdate += InventoryUpdate;
        }

        public override void OnDetach()
        {
            CustomItemsPatch.AfterInventoryUpdate -= InventoryUpdate;
            ModifyCount(ability._count);
        }

        private void ModifyCount(int amount)
        {
            var bone = owner.GetComponentInChildren<Bone>();

            if (bone == null)
            {
                return;
            }

            _bonePresent = true;
            ModifyCount(bone, amount);
        }

        private void ModifyCount(Bone bone, int amount)
        {
            bone._buff.cycle += amount;
        }

        private void InventoryUpdate(Inventory inventory)
        {
            if (_bonePresent)
            {
                return;
            }

            var bone = owner.GetComponentInChildren<Bone>();

            if (bone == null)
            {
                _bonePresent = false;
                return;
            }

            _bonePresent = true;
            ModifyCount(bone, -ability._count);
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
        return new ReduceBoneInscriptionRequirement()
        {
            _count = _count,
            _defaultIcon = _defaultIcon,
        };
    }
}
