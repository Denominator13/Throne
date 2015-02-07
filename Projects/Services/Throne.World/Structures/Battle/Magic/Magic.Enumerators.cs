using System;

namespace Throne.World.Structures.Battle
{
    public partial class Magic
    {
        public enum CastState
        {
            Preparing,
            Ready,
            Targeting,
            /// <summary>
            ///  intensify, spirit focus, inferno
            /// </summary>
            Focusing,
            /// <summary>
            /// pervade
            /// </summary>
            Channeling,
            Performing,
            Finished,
            
            NotEnoughMana,
            NotEnoughStamina,
            NotEnoughSpecial
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