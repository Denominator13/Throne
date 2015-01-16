using System;
using System.Net;
using Throne.Framework;
using Throne.Framework.Configuration;
using Throne.Framework.Configuration.Interfaces;
using Throne.World.Properties;

namespace Throne.World.Configuration.Files
{
    public class NetworkConfigFile : ConfigurationFile, INetworkConfiguration
    {
        private IPAddress _host;

        public Int32 Backlog { get; private set; }
        public String Cast5Standard { get; private set; }
        public Int32 FirewallMaxConnections { get; private set; }
        public Int32 FirewallSeconds { get; private set; }
        public Boolean GracefulDisconnect { get; private set; }
        public Boolean KeepAlive { get; private set; }
        public Boolean NoFragment { get; private set; }
        public Boolean NoNagel { get; private set; }
        public String PacketIncomingFooter { get; private set; }
        public String PacketOutgoingFooter { get; private set; }
        public Int32 Port { get; private set; }
        public Boolean ReuseEndpoint { get; private set; }

        public IPAddress Host
        {
            get { return _host; }
        }

        public IPEndPoint IpEndPoint
        {
            get { return new IPEndPoint(Host, Port); }
        }

        public void Load()
        {
            string cfgFile = "system/config/{0}/Network.cfg".Interpolate(WorldConfiguration.ServerName);
            Require(cfgFile);

            if (!IPAddress.TryParse(GetString("host"), out _host))
                throw new ConfigurationValueException(StrRes.SMSG_HostInvalid.Interpolate(cfgFile));

            if (!(Port = GetInt("port")).IsBetween(IPEndPoint.MinPort, IPEndPoint.MaxPort))
                throw new ConfigurationValueException(StrRes.SMSG_PortInvalid.Interpolate(cfgFile));

            if (!(Backlog = GetInt("backlog")).IsBetween(10, 100))
                throw new ConfigurationValueException(StrRes.SMSG_BacklogRecommend.Interpolate(cfgFile));

            PacketOutgoingFooter = GetString("outgoing_packet_footer", "TQServer");
            PacketIncomingFooter = GetString("incoming_packet_footer", "TQClient");

            if (string.IsNullOrEmpty(PacketOutgoingFooter) || !PacketOutgoingFooter.Length.Equals(8))
                throw new ConfigurationValueException(StrRes.SMSG_OutgoingFooterInvalid.Interpolate(cfgFile));

            if (string.IsNullOrEmpty(PacketIncomingFooter) || !PacketIncomingFooter.Length.Equals(8))
                throw new ConfigurationValueException(StrRes.SMSG_IncomingFooterInvalid);

            FirewallSeconds = GetInt("firewall_reset_seconds", 10);
            FirewallMaxConnections = GetInt("firewall_max_connections", 10);
            Cast5Standard = GetString("cast5_standard");
            NoNagel = GetBool("no_nagel");
            KeepAlive = GetBool("keep_alive");
            GracefulDisconnect = GetBool("graceful");
            NoFragment = GetBool("no_fragment");
            ReuseEndpoint = GetBool("reuse_endpoint");
        }
    }
}