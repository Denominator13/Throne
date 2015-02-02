using Throne.Framework.Collections;
using Throne.World.Structures.Battle.Targeting;
using Throne.World.Structures.Objects;
using Throne.World.Structures.Storage;

namespace Throne.World.Structures.Battle
{
    public partial class Magic
    {
        public readonly BooleanArray<ResponseEffect> Effects;
        public readonly SkillStorage Skills;
        public readonly TargetingCollection Targeting;

        public Role Caster
        {
            get { return Targeting.Caster; }
            internal set { Targeting.Caster = value; }
        }

        public MagicSkill MagicSkill
        {
            get { return Targeting.Skill; }
            internal set { Targeting.Skill = value; }
        }

        public BattleInteraction Usage
        {
            get { return Targeting.Usage; }
            private set { Targeting.Usage = value; }
        }

        public CastState State { get; internal set; }
    }
}