using System;
using System.Collections.Generic;
using Throne.Framework;
using Throne.Framework.Network.Transmission.Stream;
using Throne.World.Database.Client;
using Throne.World.Network;
using Throne.World.Network.Messages;
using Throne.World.Records;
using Throne.World.Scripting.Scripts;
using Throne.World.Structures.Storage;

namespace Throne.World.Structures.Objects
{
    public partial class Item : WorldObject
    {
        public readonly ItemRecord Record;

        private ItemScript _script;

        public Item(ItemRecord record)
            : base(record.Guid)
        {
            Record = record;
        }

        public ItemScript Script
        {
            get
            {
                if (!_script)
                    _script = ScriptManager.Instance.GetItemScript(this);
                return _script;
            }
        }

        public UInt32 Guid
        {
            get { return Record.Guid; }
        }

        public CharacterRecord OwnerInfo
        {
            get { return Record.Owner; }
            set
            {
                if (Record.Owner)
                    if (Record.Owner != value)
                        Record.Owner.ItemPayload.Remove(Record);

                Record.Owner = value;
                Record.Update();
            }
        }


        public Int32 Type
        {
            get { return Record.Type; }
        }

        public Int32 FusionType
        {
            get { return 50000; //fused with speed arrows for now ;D
            }
        }

        public ItemTemplate Base
        {
            get
            {
                if (!ItemManager.Templates.ContainsKey(Type))
                    throw new NotSupportedException(
                        "Item Type {0} does not have a template.".Interpolate(Type));

                return ItemManager.Templates[Type];
            }
        }

        public ItemTemplate FusedItem
        {
            get
            {
                if (FusionType == 0)
                    return null;

                if (!ItemManager.Templates.ContainsKey(FusionType))
                    throw new NotSupportedException(
                        "Item {0}'s fused type ({1}) has no template.".Interpolate(Guid, FusionType));

                return ItemManager.Templates[Type];
            }
        }


        public Positions Position
        {
            get { return Record.Position; }
            set
            {
                Record.Position = value;
                Record.Update();
            }
        }

        public DepositoryType DepositoryType
        {
            get { return Record.DepositoryType; }
            set
            {
                Record.DepositoryType = value;
                Record.Update();
            }
        }

        public DepositoryId DepositoryId
        {
            get { return Record.DepositoryId; }
            set { Record.DepositoryId = value; }
        }


        public static Stream ToStream(IEnumerable<Item> toSend)
        {
            var _stream = new Stream();
            foreach (Item item in toSend)
                _stream.Join(item);
            return _stream;
        }

        public static implicit operator Boolean(Item item)
        {
            return item != null;
        }

        public static implicit operator Byte[](Item toAdd)
        {
            return new ItemInformation(toAdd);
        }

        public override void SpawnFor(WorldClient observer)
        {
            using (var pkt = new MapItemInformation(this))
                observer.Send(pkt);
        }

        public override void DespawnFor(WorldClient observer)
        {
            using (var pkt = new MapItemInformation(this, true))
                observer.Send(pkt);
        }
    }
}