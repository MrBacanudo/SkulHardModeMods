using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Weapons;
using UnityEngine;

namespace CustomItems.CustomAbilities;

[Serializable]
public class GravityScale : Ability, ICloneable
{
    public class Instance : AbilityInstance<GravityScale>
    {
        public Instance(Character owner, GravityScale ability) : base(owner, ability)
        {
        }

        private void WeaponChange(Weapon w1, Weapon w2)
        {
            // Damn you, Gargoyle!
            var override1 = w1?.GetComponentInChildren<OverrideMovementConfigComponent>(true);
            if (override1 != null)
            {
                override1._ability._config.gravity /= ability.amount;
            }

            var override2 = w2?.GetComponentInChildren<OverrideMovementConfigComponent>(true);
            if (override2 != null)
            {
                override2._ability._config.gravity *= ability.amount;
            }
        }

        public override void OnAttach()
        {
            foreach (var config in owner.movement.configs)
            {
                config.gravity *= ability.amount;
            }
            owner.playerComponents.inventory.weapon.onChanged += WeaponChange;
        }

        public override void OnDetach()
        {
            foreach (var config in owner.movement.configs)
            {
                config.gravity /= ability.amount;
            }
            owner.playerComponents.inventory.weapon.onChanged -= WeaponChange;
        }
    }


    [SerializeField]
    internal float amount = 1.0f;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new GravityScale()
        {
            amount = amount
        };
    }
}
