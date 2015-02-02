using System.Collections.Generic;
using Throne.Framework;
using Throne.World.Structures.Objects;

namespace Throne.World.Structures.Battle.Targeting
{
    public class TargetingCollection : List<WorldObject>
    {
        public Role Caster;
        public MagicSkill Skill;
        public BattleInteraction Usage;


        /// <summary>
        ///     Add a target to the collection with message.
        /// </summary>
        /// <param name="target"></param>
        public new void Add(WorldObject target)
        {
            target.Send("You're being targeted by {0}.".Interpolate(Caster));
            base.Add(target);
        }

        public void Collect()
        {
            var result = CollectorResult.Failed;
            foreach (var collector in Skill.Template.TargetCollector.Collectors)
                collector(this, ref result);
        }
    }
}