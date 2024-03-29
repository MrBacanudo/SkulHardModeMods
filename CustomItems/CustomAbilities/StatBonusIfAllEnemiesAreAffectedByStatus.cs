using System;
using System.Linq;
using Characters;
using Characters.Abilities;
using Data;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CustomItems.CustomAbilities;

[Serializable]
public class StatBonusIfAllEnemiesAreAffectedByStatus : Ability, ICloneable
{
    public class Instance : AbilityInstance<StatBonusIfAllEnemiesAreAffectedByStatus>
    {
        private const float _updateInterval = 0.1f;

        private float _remainUpdateTime;

        private Stat.Values _stat;

        private CharacterStatus.Kind _status;

        private bool _active = false;

        public override Sprite icon => _active ? base.icon : null;

        public Instance(Character owner, StatBonusIfAllEnemiesAreAffectedByStatus ability)
            : base(owner, ability)
        {
            _status = ability._status;
            _stat = ability._stats.Clone();
        }

        public override void OnAttach()
        {
            UpdateStat();
        }

        public override void OnDetach()
        {
            if (_active)
            {
                _active = false;
                owner.stat.DetachValues(_stat);
            }
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
            bool activate = AllEnemiesAreAffected();
            if (!_active && activate)
            {
                _active = true;
                owner.stat.AttachValues(_stat);
            }
            else if (_active && !activate)
            {
                _active = false;
                owner.stat.DetachValues(_stat);
            }
        }

        private bool AllEnemiesAreAffected()
        {
            var enemies = UnityEngine.Object.FindObjectsOfType<Character>()
                         .Where(character => character.health != null && !character.health.dead &&
                                (character.type != Character.Type.Player) && (character.type != Character.Type.PlayerMinion) && (character.type != Character.Type.Trap))
                         .ToArray();

            var affectedEnemies = enemies.Count(enemy => enemy.status != null && enemy.status.IsApplying(_status));

            return affectedEnemies > 0 && affectedEnemies == enemies.Length;
        }
    }

    [SerializeField]
    public Stat.Values _stats;

    [SerializeField]
    public CharacterStatus.Kind _status;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new StatBonusIfAllEnemiesAreAffectedByStatus()
        {
            _stats = _stats.Clone(),
            _defaultIcon = _defaultIcon,
            _status = _status,
        };
    }
}
