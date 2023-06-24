using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace CustomItems.CustomAbilities;

[Serializable]
public class StatBonusPerManatechPart : Ability, ICloneable
{
    public class Instance : AbilityInstance<StatBonusPerManatechPart>
    {
        private Stat.Values _stats;

        private float _bonusTimeRemaining;
        public override float iconFillAmount => 1.0f - _bonusTimeRemaining / ability._timeout;

        int _stacks;
        public override int iconStacks => _stacks;
        public override Sprite icon
        {
            get
            {
                if (_stacks == 0)
                {
                    return null;
                }
                return ability._defaultIcon;
            }
        }

        public Instance(Character owner, StatBonusPerManatechPart ability) : base(owner, ability)
        {
        }

        private void ManatechPickup()
        {
            _stacks = Math.Min(_stacks + 1, ability._maxStack);
            _bonusTimeRemaining = ability._timeout;
            RefreshStacks();
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            if (_stacks == 0)
            {
                return;
            }

            _bonusTimeRemaining -= deltaTime;

            if (_bonusTimeRemaining < 0f)
            {
                _stacks = 0;
                RefreshStacks();
            }
        }

        private void RefreshStacks()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = ability._statPerStack.values[i].GetStackedValue(_stacks);
            }
            owner.stat.SetNeedUpdate();
        }

        public override void OnAttach()
        {
            _stacks = 0;
            _stats = ability._statPerStack.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStacks();
            CustomItemsPatch.OnManatechPart += ManatechPickup;
        }

        public override void OnDetach()
        {
            CustomItemsPatch.OnManatechPart -= ManatechPickup;
            owner.stat.DetachValues(_stats);
        }
    }


    [SerializeField]
    internal float _timeout = 1;

    [SerializeField]
    internal int _maxStack = 1;

    [SerializeField]
    internal Stat.Values _statPerStack;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new StatBonusPerManatechPart()
        {
            _timeout = _timeout,
            _maxStack = _maxStack,
            _statPerStack = _statPerStack.Clone(),
            _defaultIcon = _defaultIcon,
        };
    }
}
