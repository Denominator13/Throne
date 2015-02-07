using System;
using Throne.World.Structures.Battle.Targeting.Filters;
using Throne.World.Structures.Objects;

namespace Throne.World.Structures.Battle.Targeting
{
    public abstract class TargetEvaluation : TargetFilter
    {
    }

    /// <summary>
    ///     Brings the nearest WorldObjects to the top of the list.
    /// </summary>
    public class NearestTargetEvaluator : TargetEvaluation
    {
        public override void Invoke(Magic magic)
        {
            var usage = magic.Usage;
            magic.Targets.Sort((a, b) => Distance(usage, a).CompareTo(Distance(usage, b)));
        }

        private static Int32 Distance(BattleInteraction usage, IWorldObject target)
        {
            return target.Location.Position.GetDistance(usage.TargetLocation);
        }
    }
}