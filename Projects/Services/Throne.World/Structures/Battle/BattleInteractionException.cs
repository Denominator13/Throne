using System;

namespace Throne.World.Structures.Battle
{
    public sealed class BattleInteractionException : Exception
    {
        public BattleInteractionException(String message) : base(message)
        {
        }
    }
}