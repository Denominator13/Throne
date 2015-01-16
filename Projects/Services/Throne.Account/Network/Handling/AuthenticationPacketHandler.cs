using System;
using System.Reflection;
using Throne.Framework.Network.Handling;
using Throne.Framework.Network.Transmission;

namespace Throne.Login.Network.Handling
{
    public class AuthenticationPacketHandler : PacketHandlerBase
    {
        public AuthenticationPacketHandler(ConstructorInfo constructor, Enum typeId, Type permission)
            : base(constructor, typeId, permission)
        {
        }

        public void Invoke(LoginClient client, params object[] param)
        {
            var processor = (LoginPacket)Constructor.Invoke(param);

            if (processor.Read(client))
                processor.Handle(client);

            processor.Dispose();
        }
    }
}