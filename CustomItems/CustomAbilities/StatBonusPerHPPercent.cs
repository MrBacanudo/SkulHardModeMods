using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace CustomItems.CustomAbilities;

[Serializable]
public class StatBonusPerHPPercent : Ability, ICloneable
{
    public class Instance : AbilityInstance<StatBonusPerHPPercent>
    {
        private Stat.Values _stats;

        public override int iconStacks => (int)owner.health.percent * 100;

        public Instance(Character owner, StatBonusPerHPPercent ability) : base(owner, ability)
        {
        }

        private void RefreshStacks()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = ability._maxStat.values[i].GetMultipliedValue((float)owner.health.percent);
            }
            owner.stat.SetNeedUpdate();
        }

        public override void OnAttach()
        {
            _stats = ability._maxStat.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStacks();
            owner.health.onChanged += RefreshStacks;
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.health.onChanged -= RefreshStacks;
        }
    }

    [SerializeField]
    internal Stat.Values _maxStat;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new StatBonusPerHPPercent()
        {
            _maxStat = _maxStat.Clone(),
            _defaultIcon = _defaultIcon,
        };
    }
}
