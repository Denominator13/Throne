using System;
using Throne.Login.Network;

namespace Throne.Framework.Network.Transmission
{
    public class LoginPacket : Packet
    {
        protected LoginPacket()
        {
        }

        public LoginPacket(IConvertible type, short len)
            : base(type, len)
        {
        }

        public LoginPacket(byte[] payload)
            : base(payload)
        {
        }

        public virtual Boolean Read(LoginClient client)
        {
            return false;
        }

        public virtual void Handle(LoginClient client)
        {
        }
    }
}