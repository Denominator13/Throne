using Throne.World.Structures.Battle.Targeting;
using Throne.World.Structures.Objects;
using Throne.World.Structures.Storage;

namespace Throne.World.Structures.Battle
{
    public partial class Magic
    {
        public readonly SkillStorage Skills;
        public readonly TargetList Targets;

        public Role Caster { get; internal set; }
        public MagicSkill CurrentSkill { get; internal set; }
        public BattleInteraction Usage { get; private set; }
        public CastState State { get; internal set; }
    }
}