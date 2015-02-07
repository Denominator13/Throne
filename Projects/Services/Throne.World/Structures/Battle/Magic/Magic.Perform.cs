using System;
using Throne.Framework;
using Throne.World.Security;
using Throne.World.Structures.Objects;

namespace Throne.World.Structures.Battle
{
    public partial class Magic
    {
        private void Initialize()
        {
            if (State == CastState.Focusing)
                throw new NotImplementedException("TODO: Focused skill not yet cancelled.");

            State = CastState.Preparing;
            CurrentSkill = Skills[Usage.Type];
            if (CurrentSkill == null)
                throw new ModerateViolation("Used unattained skill {0}.", Usage.Type);
            if (CurrentSkill.Level != Usage.Level)
                throw new MildViolation("Used unattained skill level {0} (highest: {1})", Usage.Level,
                    CurrentSkill.Level);
        }

        private void Performable()
        {
            // todo if (mana < neededMana) throw new BattleInteractionException("You do not have enough mana.");
            State = CastState.Ready;
        }

        private void Targeting()
        {
            State = CastState.Targeting;
            CurrentSkill.Template.TargetCollector.Invoke(this);

            if (Targets.Count == 0)
                throw new BattleInteractionException("Could not find a target.");

            Caster.Send("Targeted {0}".Interpolate(string.Join(",", Targets)));
        }

        private void Perform()
        {
            State = CastState.Performing;

            var chr = Caster as Character;
            if (chr)
                chr.SendToLocal(Targets, true);

            //channel if the skill is channeled
            //calculate damage
            //deal damage
            //send usage and effect
        }
    }
}