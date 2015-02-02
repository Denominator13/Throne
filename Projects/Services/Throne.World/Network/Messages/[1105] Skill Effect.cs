using System;
using Throne.Framework.Network.Transmission;
using Throne.World.Structures.Objects;

namespace Throne.World.Network.Messages
{
    public class SkillEffect : WorldPacket
    {
        public SkillEffect(UInt32 caster, ushort x, ushort y, Int32 skill, ushort level, params IWorldObject[] targets)
            : base(PacketTypes.SkillEffect, 20 + 32*targets.Length + 8)
        {
            WriteUInt(caster);
            WriteUShort(x);
            WriteUShort(y);
            WriteUShort((ushort) skill);
            WriteUShort(level);
            WriteUShort(0); // no idea what this guy is for.
            WriteByte(0); // current soul
            WriteByte((byte) targets.Length); // target count

            foreach (IWorldObject target in targets)
            {
                WriteUInt(target.ID); // target
                WriteInt(1); // effect value
                WriteInt(2); // KO? no idea
                WriteInt(0); // effect flags (crit, crash, study points)
                WriteInt(0); // flag value
                WriteInt(0); // X?
                WriteInt(0); // Y?
                WriteInt(0); // unknown.. setting a value here causes the attack to show no value.\
            }
        }
    }
}