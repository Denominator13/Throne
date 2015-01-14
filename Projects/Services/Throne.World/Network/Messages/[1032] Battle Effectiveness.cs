using System;
using Throne.Framework.Network.Transmission;

namespace Throne.World.Network.Messages
{
    public sealed class BattleEffectiveness : WorldPacket
    {
        public BattleEffectiveness(UInt32 id) : base(PacketTypes.BattleEffectiveness, 15 + 8)
        {
            WriteUInt(id);
            WriteUShort(0); // battlepower
        }
    }
}