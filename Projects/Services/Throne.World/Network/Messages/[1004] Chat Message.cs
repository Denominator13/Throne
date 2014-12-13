﻿using System;
using Throne.Shared.Network.Connectivity;
using Throne.Shared.Network.Transmission;
using Throne.World.Network.Handling;
using Throne.World.Properties.Settings;
using Throne.World.Structures.Objects;

namespace Throne.World.Network.Messages
{
    [WorldPacketHandler(PacketTypes.ChatMessage)]
    public class ChatMessage : WorldPacket
    {
        public const String SYSTEM = "SYSTEM", ALLUSERS = "ALLUSERS";
        private const Int32 MINIMUM_LENGTH = 33, AMOUNT_OF_STRINGS = 4;

        public MessageColor Color;

        public WorldClient Client;

        public UInt32 Identity;
        public String Message;
        public String MessageSuffix = "";
        public String Recipient = "";
        public UInt32 RecipientMesh;
        public String Sender = "";
        public UInt32 SenderMesh;
        public MessageStyle Style;
        public MessageChannel Type;

        public ChatMessage(MessageChannel type, String message)
            : base(0)
        {
            TypeId = (short)PacketTypes.ChatMessage;

            Type = type;
            Style = MessageStyle.Normal;
            Color = MessageColor.White;
            Sender = SYSTEM;
            Recipient = ALLUSERS;
            Message = message;
        }

        public ChatMessage(MessageChannel type, String message, Role to)
            : base(0)
        {
            TypeId = (short)PacketTypes.ChatMessage;

            Type = type;
            Style = MessageStyle.Normal;
            Color = MessageColor.White;
            Sender = SYSTEM;
            Identity = to.ID;
            Recipient = to.Name;
            Message = message;
        }

        public ChatMessage(MessageChannel type, String message, MessageStyle style, MessageColor color)
            : base(0)
        {
            TypeId = (short)PacketTypes.ChatMessage;

            Type = type;
            Style = style;
            Color = color;
            Sender = SYSTEM;
            Recipient = ALLUSERS;
            Message = message;
            MessageSuffix = "asdfg";
        }



        /// <summary>
        ///     Incoming constructor.
        /// </summary>
        /// <param name="array">Incoming byte array.</param>
        public ChatMessage(byte[] array)
            : base(array)
        {
        }

        public override bool Read(IClient client)
        {
            Client = (WorldClient) client;

            ReadInt();
            Color = (MessageColor)ReadInt();
            Type = (MessageChannel)ReadUShort();
            Style = (MessageStyle)ReadUShort();
            Identity = ReadUInt();
            RecipientMesh = ReadUInt();
            SenderMesh = ReadUInt();

            var strings = ReadStrings();

            if (strings.Length < AMOUNT_OF_STRINGS)
            {
                InvalidValue(client, StrRes.SMSG_ChatMessageBadStrings, strings.Length, false);
                return false;
            }

            Sender = strings[0];
            Recipient = strings[1];
            MessageSuffix = strings[2];
            Message = strings[3];

            if (Sender != Client.Character.Name)
            {
                InvalidValue(client, StrRes.SMSG_ChatMessageInvalidSender, Sender, Client.Character.Name, false);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(Message)) return true;
#if DEBUG
            Log.Info(StrRes.SMSG_ChatMessageEmpty);
#endif
            return false;
        }

        public override void Handle(IClient client)
        {
            Seek(4);
            ChatManager.Instance.PostAsync(cm => cm.ProcessChatMessage(this));
        }

        protected override byte[] Build()
        {
            Resize(MINIMUM_LENGTH + Recipient.Length + Sender.Length + MessageSuffix.Length + Message.Length + 8);
            WriteInt(Environment.TickCount);
            WriteInt((Int32)MessageColor.Green);
            WriteUShort((ushort)Type);
            WriteUShort((ushort)Style);
            WriteUInt(Identity);
            WriteUInt(RecipientMesh);
            WriteUInt(SenderMesh);
            WriteByte(AMOUNT_OF_STRINGS);
            WriteStringWithLength(Sender);
            WriteStringWithLength(Recipient);
            WriteStringWithLength(MessageSuffix);
            WriteStringWithLength(Message);
            return base.Build();
        }
    }

    public enum MessageChannel : ushort
    {
        Talk = 2000,
        Whisper = 2001,
        Action = 2002,
        Team = 2003,
        Guild = 2004,
        TopLeft = 2005,
        Clan = 2006,
        System = 2007,
        Yell = 2008,
        Friends = 2009,
        Center = 2011,
        Ghost = 2013,
        Service = 2014,
        Tip = 2015,
        Horn = 2016,
        ServerBroadcast = 2017,
        World = 2021,
        ArenaQualifier = 2022,
        Study = 2024,
        Ally = 2025,
        JiangHu = 2026,

        /// <summary>  Used for message popups in-game and at character creation. </summary>
        Popup = 2100,
        Login = 2101,
        Shop = 2102,
        Pet = 2103,
        Cryout = 2104,
        Website = 2105,
        StartRight = 2108,
        ContinueRight = 2109,
        OfflineWhisper = 2110,
        GuildBulletin = 2111,
        SystemCenter = 2115,
        TradeBoard = 2201,
        FriendBoard = 2202,
        TeamBoard = 2203,
        GuildBoard = 2204,
        OtherBoard = 2205,
        BroadcastMessage = 2500,
        MonsterTalk = 2600
    }

    [Flags]
    public enum MessageStyle : ushort
    {
        Normal = 0,
        Scroll = 1 << 0,
        Flash = 1 << 1,
        Blast = 1 << 2
    }

    public enum MessageColor
    {
        White = 0xFFFFFF,
        Black = 0x000000,
        Yellow = 0x00FFFF,
        Pink = 0xFF00FF,
        Green = 0x00FF00,
        Blue = 0x0000FF,
        Red = 0xFF0000
    }
}