using System;
using Throne.Framework.Network.Transmission;
using Throne.World.Structures.Battle;

namespace Throne.World.Network.Messages
{
    public class SkillInformation : WorldPacket
    {
        [Flags]
        public enum SkillSoul
        {
            None = 0,
            Available = 0x0,
            First = 0x1,
            Second = 0x2,
            Third = 0x4,
            Fourth = 0x8
        }

        public enum SkillSoulAction
        {
            None,
            Unknown,
            Assign,
            Confirm,
            Update,
            Remove
        }

        public Int32 ActiveSoul;

        public UInt32 Experience;
        public UInt16 Level;
        public UInt16 SkillId;
        public SkillSoulAction SoulAction;
        public Int32 Timestamp;
        public UInt16 Unknown;


        public SkillInformation(MagicSkill skill)
            : base(PacketTypes.SkillInformation, 32 + 8)
        {
            WriteInt(Environment.TickCount);
            WriteUInt(skill.Experience);
            WriteInt(skill.Type);
            WriteUShort(skill.Level);
            WriteUShort(0);// soul action
            WriteInt(0);// current soul
            WriteInt(0);// flags; bound souls
            WriteInt(0);// flags; unlock bound souls
        }

        public override bool Read(WorldClient client)
        {
            Timestamp = ReadInt();
            Experience = ReadUInt();
            SkillId = ReadUShort();
            Unknown = ReadUShort();
            Level = ReadUShort();
            SoulAction = (SkillSoulAction) ReadUShort();
            ActiveSoul = ReadInt();
            return true;
        }
    }
}