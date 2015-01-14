using Throne.Framework.Network.Transmission;

namespace Throne.World.Network.Messages
{
    public sealed class VIPValidFunctions : WorldPacket
    {
        public VIPValidFunctions() : base(PacketTypes.VipValidFunctions, 16)
        {
            //ushort boolean array...
            WriteInt(32767);
        }
    }
}