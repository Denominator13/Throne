
using Throne.Framework.Network.Connectivity;

namespace Throne.Framework.Network.Handling
{
    public interface IPacketPropagator
    {
        void Handle<TClient>(TClient client, short typeId, byte[] payload, short length) where TClient : IClient;
    }
}