using System;
using System.Linq;
using Characters;
using Characters.Abilities;
using Characters.Gear.Synergy.Inscriptions;
using Data;
using UnityEngine;

namespace CustomItems.CustomAbilities;

[Serializable]
public class InscriptionCountAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<InscriptionCountAbility>
    {
        private EnumArray<Inscription.Key, Inscription> inscriptions;

        public override int iconStacks
        {
            get
            {
                return inscriptions.Count((inscription) => inscription.count > 0);
            }
        }

        public Instance(Character owner, InscriptionCountAbility ability)
            : base(owner, ability)
        {
            inscriptions = owner.playerComponents.inventory.synergy.inscriptions;
        }

        public override void OnAttach()
        {
        }

        public override void OnDetach()
        {
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new InscriptionCountAbility()
        {
            _defaultIcon = _defaultIcon,
        };
    }
}
