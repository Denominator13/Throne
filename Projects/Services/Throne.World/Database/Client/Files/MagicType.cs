using System;
using System.Runtime.InteropServices;
using Throne.World.Structures.Battle.Targeting;

namespace Throne.World.Database.Client.Files
{
    [StructLayout(LayoutKind.Sequential)]
    public class MagicTemplate : IClientDbRecord
    {
        public int Id { get; set; }

        public readonly Int32 Type;
        
        public readonly MagicSort Sort;

        public readonly String Name;
        public readonly Boolean Crime, Ground, MultiTarget;

        public readonly Int32
            Target,
            Level,
            ManaUsage,
            Power,
            IntoneSpeed,
            EffectChance,
            StepSeconds,
            Range,
            Distance,
            Unknown,
            Status,
            RequiredProficiency,
            RequiredExperience,
            RequiredLevel;

        public Single EffectPercent
        {
            get { return (Single) Power%10000/100; }
        }

        public TargetCollector TargetCollector { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public enum MagicSort
    {
        SingleTarget = 1
    }
}