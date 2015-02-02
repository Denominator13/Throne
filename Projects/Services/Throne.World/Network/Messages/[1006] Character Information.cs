﻿using System;
using Throne.Framework.Network.Transmission;
using Throne.World.Structures.Objects;

namespace Throne.World.Network.Messages
{
    public sealed class CharacterInformation : WorldPacket
    {
        private const int MINIMUM_LENGTH = 138;

        #region Fields

        private readonly Role.Profession
            AncestorJob;

        private readonly string
            CharacterName;

        private readonly Role.Profession
            CurrentJob;

        private readonly uint
            EMoney;

        private readonly uint
            Guid;

        private readonly Role.Profession
            LastJob;

        private readonly byte
            Level;

        private readonly Model
            Model;

        private readonly uint
            Money;

        private readonly string
            SpouseName = "None";

        private ushort
            Agility;

        private ushort
            Attributes;

        private int
            BoundEMoney;

        private ushort
            CountryCode;

        private short
            CrimeLevel;

        private ushort
            EnlightenmentPoints,
            EnlightenmentTime;

        private ulong
            Experience;

        private ushort
            Hairstyle;

        private uint
            HealthPoints;

        private ushort
            ManaPoints;

        private uint
            QuizPoints,
            RacePoints;

        private byte
            Rebirth;

        private ushort
            Spirit;

        private ushort
            Strength;

        private ushort
            Title;

        private byte
            VIPLevel;

        private ushort
            Vitality;

        #endregion

        public CharacterInformation(Character character) : base(0)
        {
            TypeId = (short)PacketTypes.CharacterInformation;

            CharacterName = character.Name;
            if (character.Spouse != null)
                SpouseName = character.Spouse.Name;
            Model = character.Look;
            Guid = character.ID;
            Level = character.Level;
            CurrentJob = character.CurrentJob;
            LastJob = character.PreviousJob;
            AncestorJob = character.AncestorJob;
            Money = character.Money;
            EMoney = character.EMoney;
            HealthPoints = 9900;
            ManaPoints = 1500;
            Spirit = 300;
            Vitality = 300;
            Agility = 300;
            Strength = 300;
        }

        protected override byte[] Build()
        {
            Resize(MINIMUM_LENGTH + CharacterName.Length + SpouseName.Length);
            WriteInt(Environment.TickCount);
            WriteUInt(Guid);
            WriteUShort(0); // appearance type
            WriteInt(Model);
            WriteUShort(Hairstyle);
            WriteUInt(Money);
            WriteUInt(EMoney);
            WriteULong(Experience);
            WriteUInt(0); //deed
            WriteUInt(0); //medal
            WriteUInt(0); //medal selected
            WriteUInt(0); //virtue
            WriteUInt(0); //mete level
            WriteUShort(Strength);
            WriteUShort(Agility);
            WriteUShort(Vitality);
            WriteUShort(Spirit);
            WriteUShort(Attributes);
            WriteUInt(HealthPoints);
            WriteUShort(ManaPoints);
            WriteShort(CrimeLevel);
            WriteByte(Level);
            WriteByte((byte) CurrentJob);
            WriteByte((byte) LastJob);
            WriteByte((byte) AncestorJob);
            WriteByte(0); //nobility
            WriteByte(Rebirth);
            WriteByte(0); //unknown
            WriteUInt(QuizPoints);
            WriteUShort(EnlightenmentPoints);
            WriteUShort(EnlightenmentTime); //85
            Seek(122).WriteStrings(CharacterName, String.Empty, SpouseName);
            return base.Build();
        }
    }
}