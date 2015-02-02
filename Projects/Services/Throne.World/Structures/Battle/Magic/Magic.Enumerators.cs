using System;

namespace Throne.World.Structures.Battle
{
    public partial class Magic
    {
        public enum CastState
        {
            Finished,
            Preparing,
            Targeting,
            Focusing, //intensify, spirit focus, inferno
            Channeling, // pervade
            Performing
        }

        [Flags]
        public enum ResponseEffect
        {
            Block = 1 << 0,
            Crash = 1 << 1,
            CriticalStrike = 1 << 2,
            Unknown = 1 << 4,
            Metal = 1 << 8,
            Wood = 1 << 10,
            Fire = 1 << 20,
            Earth = 1 << 40,
            StudyPoints = 1 << 80
        }
    }
}