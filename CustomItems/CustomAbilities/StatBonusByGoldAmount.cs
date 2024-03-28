using System;
using Characters;
using Characters.Abilities;
using Data;
using UnityEngine;

namespace CustomItems.CustomAbilities;

[Serializable]
public class StatBonusByGoldAmount : Ability, ICloneable
{
    public class Instance : AbilityInstance<StatBonusByGoldAmount>
    {
        private const float _updateInterval = 0.25f;

        private float _remainUpdateTime;

        private Stat.Values _stat;

        private int _stack = -1; // Negative value guarantees correct initialization when 0

        public override int iconStacks => _stack;

        public override float iconFillAmount => 0f;

        public Instance(Character owner, StatBonusByGoldAmount ability)
            : base(owner, ability)
        {
            _stat = ability._statsPerStack.Clone();
        }

        public override void OnAttach()
        {
            owner.stat.AttachValues(_stat);
            UpdateStat();
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stat);
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);
            _remainUpdateTime -= deltaTime;
            if (_remainUpdateTime < 0f)
            {
                _remainUpdateTime = _updateInterval;
                UpdateStat();
            }
        }

        private void UpdateStat()
        {
            int stacks = (int)(GameData.Currency.gold.balance / ability._goldPerStack);
            if (stacks != _stack)
            {
                _stack = stacks;
                SetStat(_stack);
            }
        }

        private void SetStat(int stack)
        {
            Stat.Value[] values = _stat.values;
            for (int i = 0; i < values.Length; i++)
            {
                values[i].value = ability._statsPerStack.values[i].value * (double)stack;
            }
            owner.stat.SetNeedUpdate();
        }
    }

    [SerializeField]
    public Stat.Values _statsPerStack;

    [SerializeField]
    public float _goldPerStack;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new StatBonusByGoldAmount()
        {
            _statsPerStack = _statsPerStack.Clone(),
            _defaultIcon = _defaultIcon,
            _goldPerStack = _goldPerStack,
        };
    }
}
