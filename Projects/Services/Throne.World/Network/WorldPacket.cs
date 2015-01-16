using System;
using Throne.Framework.Network.Transmission;

namespace Throne.World.Network
{
    public class WorldPacket : Packet
    {
        protected WorldPacket()
        {
        }


        public WorldPacket(IConvertible type, int len)
            : base(type, len)
        {
        }

        public WorldPacket(byte[] payload)
            : base(payload)
        {
        }

        public WorldPacket(int len)
            : base(len)
        {
            Seek(4);
        }

        public virtual Boolean Read(WorldClient client)
        {
            return false;
        }

        public virtual void Handle(WorldClient client)
        {
        }

        protected override byte[] Build()
        {
            if (TypeId == 0)
                Log.Error("{0} : A type ID should be set before a packet can be built.", GetType().Name);

            WriteHeader(ArrayLength - 8);
            Seek(ArrayLength - 8).WriteString(WorldServer.Configuration.Network.PacketOutgoingFooter);

            return base.Build();
        }
    }
}