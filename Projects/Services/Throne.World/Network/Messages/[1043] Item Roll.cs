using System;
using Throne.Framework.Network.Connectivity;
using Throne.Framework.Network.Transmission;
using Throne.World.Network.Handling;

namespace Throne.World.Network.Messages
{
    [WorldPacketHandler(PacketTypes.ItemRoll)]
    public sealed class ItemRoll : WorldPacket
    {
        public enum ItemRollAction
        {
            Start = 1,
            Roll = 5,
            Abandon = 6
        }

        public ItemRoll(Byte[] array) : base(array)
        {
        }

        /// <summary>Starts an item roll.</summary>
        public ItemRoll(UInt32 itemGuid, Int32 seconds) : base(PacketTypes.ItemRoll, 34)
        {
            WriteUInt(itemGuid);
            WriteUInt(0);
            WriteInt((Int32) ItemRollAction.Start); //action
            WriteInt(0);
            WriteInt(seconds); //time in seconds
        }

        public override bool Read(WorldClient client)
        {
            var itemGuid = ReadInt();
            SeekForward(4); // unknown
            var action = (ItemRollAction) ReadInt();
            return true;
        }
    }
}