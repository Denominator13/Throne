using System;
using System.Reflection;
using Throne.Framework.Network.Handling;

namespace Throne.World.Network.Handling
{
    public class WorldPacketHandler : PacketHandlerBase
    {
        public WorldPacketHandler(ConstructorInfo constructor, Enum typeId, Type permission)
            : base(constructor, typeId, permission)
        {

        }
        public void Invoke(WorldClient client, params object[] parameters)
        {
            var processor = (WorldPacket)Constructor.Invoke(parameters);

            if (processor.Read(client))
                processor.Handle(client);

            processor.Dispose();
        }
    }
}