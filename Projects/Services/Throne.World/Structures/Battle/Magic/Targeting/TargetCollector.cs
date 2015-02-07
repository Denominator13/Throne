using System;
using System.Collections.Generic;
using System.Linq;
using Throne.World.Structures.Battle.Targeting.Filters;
using Throne.World.Structures.Objects;

namespace Throne.World.Structures.Battle.Targeting
{
    /// <summary>
    ///     <see cref="TargetCollector"/>s gather their results into a <seealso cref="TargetList" />.
    ///     After targets are gathered, all are filtered based on given <seealso cref="TargetFilter" /> constraints.
    /// </summary>
    public class TargetCollection
    {
        private readonly TargetCollector[] _collectors;
        private readonly TargetFilter[] _filters;

        public TargetCollection(CollectorType collectors, params TargetFilter[] filters)
        {
            _collectors = EnumToCollectors(collectors).ToArray();
            _filters = filters;
        }

        public CollectorResult Invoke(Magic magic)
        {
            var result = CollectorResult.Success;

            foreach (TargetCollector collector in _collectors)
            {
                if (result < CollectorResult.Success) break;
                collector(magic, ref result);
            }

            foreach (TargetFilter filter in _filters)
                filter.Invoke(magic);

            return result;
        }

        private IEnumerable<TargetCollector> EnumToCollectors(CollectorType collectors)
        {
            foreach (CollectorType t in Enum.GetValues(collectors.GetType())
                .Cast<CollectorType>()
                .Where(target => collectors.HasFlag(target)))
                switch (t)
                {
                    case CollectorType.Explicit:
                        yield return Explicit;
                        break;
                    case CollectorType.Self:
                        yield return Self;
                        break;
                }
        }

        private static void Self(Magic magic, ref CollectorResult result)
        {
            magic.Targets.Add(new Target(magic.Caster));
        }

        private static void Explicit(Magic magic, ref CollectorResult result)
        {
            BattleInteraction usage = magic.Usage;
            Role caster = magic.Caster;

            WorldObject explicitTarget;
            caster.GetVisibleObject(usage.Target, out explicitTarget);

            if (explicitTarget)
                magic.Targets.Add(new Target(explicitTarget));
            else
                result = CollectorResult.TargetDoesNotExist;
        }

        private delegate void TargetCollector(Magic magic, ref CollectorResult result);
    }

    public enum CollectorResult : byte
    {
        TargetDoesNotExist,


        /// <summary> The lowest success value. </summary>
        Success
    }

    [Flags]
    public enum CollectorType
    {
        None,
        Self,
        Explicit
    }
}