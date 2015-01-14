using System;
using System.Collections.Generic;
using Throne.Framework.Network.Connectivity;
using Throne.Framework.Network.Transmission;
using Throne.World.Network.Handling;
using Throne.World.Properties.Settings;
using Throne.World.Security;
using Throne.World.Structures.Objects;
using Throne.World.Structures.Storage;

namespace Throne.World.Network.Messages
{
    [WorldPacketHandler(PacketTypes.Depository)]
    public sealed class Depository : WorldPacket
    {
        public enum DepositoryAction
        {
            Initiate,
            ShowItem,
            WithdrawItem
        }

        private DepositoryType _activeType;
        private DepositoryAction _currentAction;

        private DepositoryId _depositoryId;
        private Int32 _depositorySize;
        private Int32 _itemCount;
        private UInt32 _itemId;


        public Depository(Byte[] array)
            : base(array)
        {
        }

        public Depository(DepositoryType type, DepositoryId id, DepositoryAction action, Int32 size, params Item[] set)
            : base(PacketTypes.Depository, 28 + set.Length*56 + 8)
        {
            if (set.Length > 17)
                throw new ArgumentOutOfRangeException("set", StrRes.SMSG_DepositoryByteLimitOverflow);

            WriteUInt((uint) id);
            SeekForward(4);
            WriteByte((byte) action);
            WriteByte((byte) (10*(byte) type));
            WriteUShort(ushort.MaxValue);
            WriteInt(size);
            WriteInt(0); // item id
            WriteInt(set.Length);
            foreach (Item item in set)
            {
                WriteUInt(item.ID);
                WriteInt(item.Type);
                WriteByte(byte.MaxValue);
                WriteByte(item.FirstSlot);
                WriteByte(item.SecondSlot);
                WriteByte(byte.MaxValue);
                WriteByte(0); // item effect
                WriteInt(int.MaxValue);
                WriteByte(item.CraftLevel);
                WriteByte(0); // bless
                WriteBoolean(false); // bound
                WriteUShort(0); // enchant amount
                WriteUShort(ushort.MaxValue);
                WriteBoolean(false); // suspicious
                WriteBoolean(false); // suspicious too
                WriteBoolean(false); // locked
                WriteByte(6); // color
                WriteInt(0); // socket progress/steed color
                WriteInt(item.CraftProgress);
                WriteUInt(0); // guild ID, for inscription
                WriteInt(0); // time remaining in seconds
                WriteInt(0); // unactivated time in minutes
                WriteShort(0); //stack size
                WriteUShort(0); //amount
                WriteUShort(0); //max amount
                SeekForward(2);
            }
        }

        public override bool Read(IClient client)
        {
            _depositoryId = (DepositoryId) ReadUInt();
            SeekForward(4);
            _currentAction = (DepositoryAction) ReadByte();
            _activeType = (DepositoryType) (ReadByte()*0.10); // div/0-safe
            SeekForward(sizeof (short)); // unknown
            _depositorySize = ReadInt();
            _itemId = ReadUInt();
            return true;
        }

        public override void Handle(IClient client)
        {
            if (!Enum.IsDefined(typeof (DepositoryType), _activeType))
                throw new ModerateViolation("Player attempted to interact with a non-existant depository type.");

            var chr = ((WorldClient) client).Character;
            ItemDepository depo;
            if (!chr.Depositories[_activeType].TryGetValue(_depositoryId, out depo))
                return;

            switch (_currentAction)
            {
                case DepositoryAction.Initiate:
                    client.Send(depo.DepositoryInformationArray(_activeType, _depositoryId));
                    break;
                case DepositoryAction.ShowItem:
                    var item = chr.GetItem(_itemId);
                    if (item)
                        chr.MoveToDepository(_activeType, _depositoryId, item);
                    else client.Respond("No such item.");
                        break;
                    case DepositoryAction.WithdrawItem:
                    if (chr.MoveFromDepository(_activeType, _depositoryId, _itemId))
                        client.Send(this);
                    break;
            }
        }
    }
}