using System;
using System.Net;

namespace Throne.Framework.Configuration.Interfaces
{
    public interface INetworkConfiguration
    {
        Int32 Backlog { get; }
        String Cast5Standard { get; }
        Int32 FirewallMaxConnections { get; }
        Int32 FirewallSeconds { get; }
        Boolean GracefulDisconnect { get; }
        Boolean KeepAlive { get; }
        Boolean NoFragment { get; }
        Boolean NoNagel { get; }
        String PacketIncomingFooter { get; }
        String PacketOutgoingFooter { get; }
        Int32 Port { get; }
        Boolean ReuseEndpoint { get; }
        IPAddress Host { get; }
        IPEndPoint IpEndPoint { get; }
    }
}