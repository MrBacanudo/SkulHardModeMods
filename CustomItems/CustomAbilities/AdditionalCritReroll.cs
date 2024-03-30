using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Weapons;
using UnityEngine;

namespace CustomItems.CustomAbilities;

// As the name implies, this gives an additional critical reroll to any kind of damage source.
// If a source can't deal crits originally, it just means "allows X source to crit" (e.g. quints and items)
// (And yes, I do have a plan for an item that allows items to deal crits.)
// This shitty implementation makes it so duplicates of the same item increase the crit chance even further.
// I don't want to bother with a correct one right now, even though it's fairly easy to hack one.
[Serializable]
public class AdditionalCritReroll : Ability, ICloneable
{
    public class Instance : AbilityInstance<AdditionalCritReroll>
    {
        public Instance(Character owner, AdditionalCritReroll ability) : base(owner, ability)
        {
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (damage.motionType == ability.motionType)
            {
                damage.critical |= MMMaths.Chance(damage.criticalChance);
            }
            return false;
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(int.MaxValue, OnGiveDamage);
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(OnGiveDamage);
        }
    }


    [SerializeField]
    internal Damage.MotionType motionType;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new AdditionalCritReroll()
        {
            motionType = motionType
        };
    }
}
