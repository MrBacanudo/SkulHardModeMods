using System;
using System.Linq;
using Characters;
using Characters.Abilities;
using Characters.Actions;
using Characters.Gear.Weapons;
using UnityEngine;
using Action = Characters.Actions.Action;

namespace CustomItems.CustomAbilities;

[Serializable]
public class ExtraDash : Ability, ICloneable
{
    public class Instance : AbilityInstance<ExtraDash>
    {
        public Instance(Character owner, ExtraDash ability) : base(owner, ability)
        {
        }

        private void PlusDash(Component obj)
        {
            var actions = obj.GetComponentsInChildren<SimpleAction>(true).Where((action) => action._type == Action.Type.Dash).ToArray();
            foreach (var action in actions)
            {
                var streak = action._cooldown.streak;
                if (streak.timeout == 0.0f)
                {
                    streak.timeout = 0.4f;
                }
                streak.timeout = (streak.timeout / (streak.count + 1)) * (streak.count + 1 + ability._count);
                streak.count += ability._count;
            }
        }

        private void MinusDash(Component obj)
        {
            var actions = obj.GetComponentsInChildren<SimpleAction>(true).Where((action) => action._type == Action.Type.Dash).ToArray();
            foreach (var action in actions)
            {
                var streak = action._cooldown.streak;
                streak.timeout = (streak.timeout / (streak.count + 1)) * (streak.count + 1 - ability._count);
                streak.count -= ability._count;
                if (streak.count < 0) { streak.count = 0; } // Just in case
            }
        }

        private void WeaponChange(Weapon w1, Weapon w2)
        {
            if (w1) MinusDash(w1);
            if (w2) PlusDash(w2);
        }

        public override void OnAttach()
        {
            PlusDash(owner);
            owner.playerComponents.inventory.weapon.onChanged += WeaponChange;
        }

        public override void OnDetach()
        {
            MinusDash(owner);
            owner.playerComponents.inventory.weapon.onChanged -= WeaponChange;
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
        return new ExtraDash()
        {
            _count = _count,
        };
    }
}
