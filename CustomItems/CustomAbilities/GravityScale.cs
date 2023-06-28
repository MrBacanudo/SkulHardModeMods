using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Weapons;
using Characters.Operations.Movement;
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
                if (w1.name.StartsWith("Gargoyle"))
                {
                    var modVelocities = w1.GetComponentsInChildren<ModifyVerticalVelocity>(true);
                    foreach (var modVelocity in modVelocities)
                    {
                        if (modVelocity._amount < 0)
                        {
                            modVelocity._amount /= ability.amount;
                        }
                    }
                }
                else
                {
                    override1._ability._config.gravity /= ability.amount;
                }
            }

            var override2 = w2?.GetComponentInChildren<OverrideMovementConfigComponent>(true);
            if (override2 != null)
            {
                if (w2.name.StartsWith("Gargoyle"))
                {
                    var modVelocities = w2.GetComponentsInChildren<ModifyVerticalVelocity>(true);
                    foreach (var modVelocity in modVelocities)
                    {
                        if (modVelocity._amount < 0)
                        {
                            modVelocity._amount *= ability.amount;
                        }
                    }
                }
                else
                {
                    override2._ability._config.gravity *= ability.amount;
                }
            }
        }

        private void AttachWeapon(Weapon w)
        {
            WeaponChange(null, w);
        }
        private void DetachWeapon(Weapon w)
        {
            WeaponChange(w, null);
        }

        public override void OnAttach()
        {
            // We'll only modify the last config, because it should always be the player's movement.
            var configs = owner.movement.configs;
            configs[configs.Count - 1].gravity *= ability.amount;

            // Deal with special cases like Gargoyles and Riders
            var weapons = owner.playerComponents.inventory.weapon.weapons;
            foreach (var weapon in weapons)
            {
                AttachWeapon(weapon);
            }

            // In case the player equips a Gargoyle when the item is already equipped
            owner.playerComponents.inventory.weapon.onChanged += WeaponChange;
        }

        public override void OnDetach()
        {
            // Undo changing the default config
            var configs = owner.movement.configs;
            configs[configs.Count - 1].gravity /= ability.amount;

            // Remove the swap watcher
            owner.playerComponents.inventory.weapon.onChanged -= WeaponChange;

            // Deal with special cases like Gargoyles and Riders, undoing after removal
            var weapons = owner.playerComponents.inventory.weapon.weapons;
            foreach (var weapon in weapons)
            {
                DetachWeapon(weapon);
            }
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
