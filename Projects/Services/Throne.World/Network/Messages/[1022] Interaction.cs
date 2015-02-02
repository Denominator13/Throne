using System;
using System.Linq.Expressions;
using Throne.Framework.Network.Transmission;
using Throne.World.Network.Handling;
using Throne.World.Structures.Battle;
using Throne.World.Structures.Travel;

namespace Throne.World.Network.Messages
{
    [WorldPacketHandler(PacketTypes.Interaction)]
    public class Interaction : WorldPacket
    {
        public enum InteractType
        {
            None = 0,
            Melee = 2,
            MarriageRequest = 8,
            MarriageAccept = 9,
            Kill = 14,
            Magic = 24,
            Reflect = 26,
            ShiftTarget = 27,
            Ranged = 28,
            MonkMelee = 34,
            MonsterHunter = 36,
            ShowClaimEMoney = 37,
            HideClaimEMoney = 38,
            ClaimEMoney = 39, // Offset 32 for value
            MerchantAccept = 40,
            MerchantRefuse = 41,
            MerchantProgress = 42,
            Scapegoat = 43,
            CounterKillSwitch = 44,
            FatalStrike = 45,
            RequestInteraction = 46,
            AcceptInteraction = 47,
            RejectInteraction = 48,
            StartInteraction = 49,
            StopInteraction = 50,
            ShowSpellUsed = 52,
            ShiftPlayer = 53, //encode magic to 28
            AzureAntiDamage = 55,
            WeaponSkill = 1000 //?! 
        }

        public Int32 Arg1, Arg2, Arg3, Arg4;
        public UInt32 ObjectId;
        public UInt16 PositionX, PositionY, SkillId, SkillLevel;
        public UInt32 TargetObjectId;
        public Int32 Timestamp;
        public InteractType Type;


        public Interaction(Byte[] array) : base(array)
        {
        }

        public Interaction() : base(PacketTypes.Interaction, 44 + 8)
        {
        }

        public override bool Read(WorldClient client)
        {
            SeekForward(4); // 2 timestamps -.- seriously??
            Timestamp = ReadInt();
            ObjectId = ReadUInt();
            TargetObjectId = ReadUInt();
            PositionX = ReadUShort();
            PositionY = ReadUShort();
            Type = (InteractType) ReadInt();

            if (Type == InteractType.Magic)
            {
                SkillId = ReadUShort();
                SkillLevel = ReadUShort();
                DecodeMagic();
                return true;
            }

            // ever used for magic receives?
            Arg1 = ReadInt();
            Arg2 = ReadInt();
            Arg3 = ReadInt();
            Arg4 = ReadInt();

            client.Send(this);
            return true;
        }

        public override void Handle(WorldClient client)
        {
            switch (Type)
            {
                case InteractType.Magic:
                    client.Character.Magic.Execute(new BattleInteraction(SkillId, SkillLevel, ObjectId, TargetObjectId,
                        new Position((short) PositionX, (short) PositionY)));
                    break;
            }
        }

        //protected override byte[] Build()
        //{
        //    Seek(4);
        //    WriteInt(Environment.TickCount);
        //}

        private void DecodeMagic()
        {
            SkillId = (ushort) (ExchangeShortBits((SkillId ^ ObjectId ^ 0x915d), 13) + 0x14be);
            SkillLevel = (ushort) ((Byte)SkillLevel ^ 0x21);
            TargetObjectId = (ExchangeLongBits(TargetObjectId, 13) ^ ObjectId ^ 0x5f2d2463) + 0x8b90b51a;
            PositionX = (ushort) (ExchangeShortBits((PositionX ^ ObjectId ^ 0x2ed6), 15) + 0xdd12);
            PositionY = (ushort) (ExchangeShortBits((PositionY ^ ObjectId ^ 0xb99b), 11) + 0x76de);
        }

        private void EncodeMagic()
        {
            
        }

        private static UInt32 ExchangeShortBits(uint data, int bits)
        {
            data &= 0xffff;
            return (((data >> bits) | (data << (16 - bits))) & 0xffff);
        }

        private static UInt32 ExchangeLongBits(uint data, int bits)
        {
            return (data >> bits) | (data << (32 - bits));
        }
    }
}