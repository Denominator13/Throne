using System;
using Throne.Login.Network;

namespace Throne.Framework.Network.Transmission
{
    public class AuthenticationPacket : Packet
    {
        protected AuthenticationPacket()
        {
        }

        public AuthenticationPacket(IConvertible type, short len)
            : base(type, len)
        {
        }

        public AuthenticationPacket(byte[] payload)
            : base(payload)
        {
        }

        public virtual Boolean Read(AuthenticationClient client)
        {
            return false;
        }

        public virtual void Handle(AuthenticationClient client)
        {
        }
    }
}