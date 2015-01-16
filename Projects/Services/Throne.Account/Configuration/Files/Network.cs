using System;
using System.Net;
using Throne.Framework;
using Throne.Framework.Configuration;
using Throne.Framework.Configuration.Interfaces;
using Throne.Login.Properties;

namespace Throne.Login.Configuration.Files
{
    public class NetworkConfigFile : ConfigurationFile, INetworkConfiguration
    {
        private IPAddress _host;
        public String GameIP;
        public UInt16 GamePort;

        public Int32 Backlog { get; private set; }

        public String Cast5Standard
        {
            get { return ""; }
        }

        public Int32 FirewallMaxConnections { get; private set; }
        public Int32 FirewallSeconds { get; private set; }
        public Boolean GracefulDisconnect { get; private set; }
        public Boolean KeepAlive { get; private set; }
        public Boolean NoFragment { get; private set; }
        public Boolean NoNagel { get; private set; }

        public String PacketIncomingFooter
        {
            get { return ""; }
        }

        public String PacketOutgoingFooter
        {
            get { return ""; }
        }

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
            string cfgFile = "system/config/{0}/Network.cfg".Interpolate(LoginConfiguration.ServerName);
            Require(cfgFile);

            if (!IPAddress.TryParse(GetString("host"), out _host))
                throw new ConfigurationValueException(StrRes.SMSG_HostInvalid.Interpolate(cfgFile));

            if (!(Port = GetInt("port")).IsBetween(IPEndPoint.MinPort, IPEndPoint.MaxPort))
                throw new ConfigurationValueException(StrRes.SMSG_PortInvalid.Interpolate(cfgFile));

            if (!(Backlog = GetInt("backlog")).IsBetween(10, 100))
                throw new ConfigurationValueException(StrRes.SMSG_BacklogRecommend.Interpolate(cfgFile));

            GameIP = GetString("game_host");
            GamePort = (ushort) GetInt("game_port");

            FirewallSeconds = GetInt("firewall_reset_seconds", 10);
            FirewallMaxConnections = GetInt("firewall_max_connections", 10);
            NoNagel = GetBool("no_nagel");
            KeepAlive = GetBool("keep_alive");
            GracefulDisconnect = GetBool("graceful");
            NoFragment = GetBool("no_fragment");
            ReuseEndpoint = GetBool("reuse_endpoint");
        }
    }
}