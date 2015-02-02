using System;
using Throne.World.Security;

namespace Throne.World.Structures.Battle
{
    public partial class Magic
    {
        private Boolean PrePerform()
        {
            if (State == CastState.Focusing)
                throw new NotImplementedException("TODO: Focused skill not yet cancelled.");

            State = CastState.Preparing;

            MagicSkill = Skills[Usage.Type];
            if (MagicSkill == null)
                throw new ModerateViolation("Used unattained skill {0}.", Usage.Type);
            if (MagicSkill.Level != Usage.Level)
                throw new MildViolation("Used unattained skill level {0} (highest: {1})", Usage.Level,
                    MagicSkill.Level);

            State = CastState.Targeting;
            Targeting.Collect();

            return true;
        }

        private Boolean CanPerform()
        {
            return true;
        }

        private void Perform()
        {
            if (Targeting.Count < 1)
                return;

            State = CastState.Performing;
            //calculate damage
            //deal damage
            //send usage and effect
        }
    }
}