using System;
using Throne.Framework.Network.Transmission;
using Throne.Framework.Security.Permissions;
using Throne.World.Network.Handling;
using Throne.World.Structures.Objects;

namespace Throne.World.Network.Messages
{
    [WorldPacketHandler(PacketTypes.MapItem, Permission = typeof (AuthenticatedPermission))]
    public sealed class MapItemInformation : WorldPacket
    {
        public enum MapItemTypes : short
        {
            None,
            DropItem,
            RemoveItem,
            ObtainItem,
            DetainItem,
            SkillEffect = 10,
            GroundEffect = 11,
            RemoveEffect = 12,
            ShowEffect = 13,
            VibrateScreen = 14,
        }

        public enum SkillTypes : short
        {
            TwilightDance = 40,
            DaggerStormV2,
            DaggerStormV3,
            DaggerStorm = 46
        }

        public Item.Color Color;
        public uint ItemId;

        public int Timestamp;
        public MapItemTypes Type;
        public uint Uid;
        public ushort X, Y;

        public MapItemInformation(byte[] payload)
            : base(payload)
        {
        }

        public MapItemInformation(Item item, Boolean remove = false) : base(PacketTypes.MapItem, 48)
        {
            WriteInt(Environment.TickCount);
            WriteUInt(item.Guid);
            WriteInt(item.Type); // item type
            WriteShort(item.Location.Position.X);
            WriteShort(item.Location.Position.Y);
            WriteShort(6); // color
            WriteInt((Int16) (remove ? MapItemTypes.RemoveItem : MapItemTypes.DropItem)); // action
            WriteUInt(0);
            WriteInt(0); // time created
            WriteInt(0); // currency value
            WriteShort(0); // item type
        }

        public MapItemInformation(MapItemTypes type, IWorldObject obj)
            : base(PacketTypes.MapItem, 48)
        {
            WriteInt(Environment.TickCount);
            WriteUInt(obj.ID);
            WriteInt(0); // item type
            WriteShort(obj.Location.Position.X);
            WriteShort(obj.Location.Position.Y);
            WriteShort(0); // color
            WriteInt((Int16)type);
        }

        public override bool Read(WorldClient client)
        {
            Timestamp = ReadInt();
            Uid = ReadUInt();
            ItemId = ReadUInt();
            X = ReadUShort();
            Y = ReadUShort();
            Color = (Item.Color) ReadUShort();
            Type = (MapItemTypes) ReadUShort();
            return true;
        }

        public override void Handle(WorldClient client)
        {
            var chr = client.Character;
            if (chr.Location.Position.X != X || chr.Location.Position.Y != Y) return;
            if (!chr.AdequateInventorySpace()) return;

            var map = chr.Location.Map;
            var item = map.GetItem(Uid);
            if (!item) return;

            item.OwnerInfo = chr.Record;

            client.Send(this);
            client.Character.SendToLocal(new MapItemInformation(MapItemTypes.ObtainItem, client.Character));
            map.RemoveItem(item);
            chr.AddItem(item);
        }
    }
}