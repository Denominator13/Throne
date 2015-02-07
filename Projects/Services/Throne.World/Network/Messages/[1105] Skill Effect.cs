using System.Collections.Generic;
using Throne.Framework.Network.Transmission;
using Throne.World.Structures.Battle;
using Throne.World.Structures.Battle.Targeting;

namespace Throne.World.Network.Messages
{
    public class SkillEffect : WorldPacket
    {
        public SkillEffect(IReadOnlyCollection<Target> targets, Magic magic)
            : base(PacketTypes.SkillEffect, 20 + 32*targets.Count + 8)
        {

            WriteUInt(magic.Caster.ID);
            WriteShort(magic.Usage.TargetLocation.X);
            WriteShort(magic.Usage.TargetLocation.Y);
            WriteUShort((ushort) magic.Usage.Type);
            WriteUShort((ushort) magic.Usage.Level);
            WriteUShort(0); // no idea what this guy is for.
            WriteByte(0); // current soul
            WriteByte((byte) targets.Count); // target count

            foreach (var target in targets)
            {
                WriteUInt(target.ID); // target
                WriteInt(target.Value); // effect value
                WriteInt(0); // KO? no idea
                WriteInt((int) target.Effects); // effect flags (crit, crash, study points)
                WriteInt(target.EffectValue); // flag value
                WriteInt(0); // X?
                WriteInt(0); // Y?
                WriteInt(0); // unknown.. setting a value here causes the attack to show no value.\
            }
        }
    }
}