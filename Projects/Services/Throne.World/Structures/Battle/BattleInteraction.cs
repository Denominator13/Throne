using System;
using Throne.World.Structures.Travel;

namespace Throne.World.Structures.Battle
{
    public struct BattleInteraction
    {
        public Int32 Type, Level;
        public UInt32 Caster, Target;
        public Position TargetLocation;

        public BattleInteraction(Int32 type, Int32 level, UInt32 caster, UInt32 target, Position location)
        {
            Type = type;
            Level = level;
            Caster = caster;
            Target = target;
            TargetLocation = location;
        }
    }
}