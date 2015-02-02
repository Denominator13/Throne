using Throne.World.Database.Client.Files;
using Throne.World.Structures.Battle.Targeting.Filters;
using Throne.World.Structures.Objects;

namespace Throne.World.Structures.Battle.Targeting
{
    public class TargetCollector
    {
        public delegate void Collector(
            TargetingCollection targeting, ref CollectorResult result);

        public readonly Collector[] Collectors;
        public TargetFilter[] Filters;

        public TargetCollector(params Collector[] collectors)
        {
            Collectors = collectors;
        }

        public TargetCollector AddFilters(params TargetFilter[] filters)
        {
            Filters = filters;
            return this;
        }

        public static void Self(TargetingCollection targeting, ref CollectorResult result)
        {
            BattleInteraction usage = targeting.Usage;

            targeting.Add(targeting.Caster);

            foreach (var targetFilter in targeting.Skill.Template.TargetCollector.Filters)
                targetFilter.Invoke(usage, ref targeting);

            result = CollectorResult.Success;
        }

        public static void Explicit(TargetingCollection targeting, ref CollectorResult result)
        {
            BattleInteraction usage = targeting.Usage;
            Role caster = targeting.Caster;

            WorldObject explicitTarget;
            caster.GetVisibleObject(usage.Target, out explicitTarget);

            if (explicitTarget)
                targeting.Add(explicitTarget);
            else return;

            foreach (var targetFilter in targeting.Skill.Template.TargetCollector.Filters)
                targetFilter.Invoke(usage, ref targeting);

            result = CollectorResult.Success;
        }
    }

    public enum CollectorResult : byte
    {
        Failed,
        Success
    }
}