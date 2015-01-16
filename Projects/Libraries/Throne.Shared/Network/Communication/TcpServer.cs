using System;
using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;
using Throne.Framework.Configuration.Interfaces;
using Throne.Framework.Exceptions;
using Throne.Framework.Network.Connectivity;
using Throne.Framework.Network.Handling;
using Throne.Framework.Network.Security.Firewall;

namespace Throne.Framework.Network.Communication
{
    public sealed class TcpServer : Socket
    {
        private IPacketPropagator _propagator;

        public TcpServer()
            : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
        }

        #region Server Information

        /// <summary>
        ///     The name of the server.
        /// </summary>
        public String Name { [UsedImplicitly] get; private set; }

        /// <summary>
        ///     The text for each outgoing packet footer.
        /// </summary>
        public static String OutgoingFooter { get; private set; }

        /// <summary>
        ///     Footer length for outgoing packets.
        /// </summary>
        public static Int16 OutgoingFooterLength { get; private set; }

        /// <summary>
        ///     The text for each incoming packet footer.
        /// </summary>
        public static String IncomingFooter { get; private set; }

        /// <summary>
        ///     Footer length for incoming packets.
        /// </summary>
        public static Int16 IncomingFooterLength { get; private set; }

        #endregion

        /// <summary>
        ///     An interface to Windows Firewall, will protect the server by adding rules
        ///     to the hnetcfg.fwpolicy2. As such, it blocks server ports from malicious IP addresses.
        /// </summary>
        private NetFwHandler NetFw { get; set; }

        public IPEndPoint EndPoint { get; private set; }

        public static event EventHandler<ConnectionEventArgs> ClientConnected;
        public static event EventHandler<ConnectionEventArgs> ClientDisconnected;

        public void Start(IPacketPropagator packetPropagator, INetworkConfiguration cfg)
        {
            _propagator = packetPropagator;
            OutgoingFooter = cfg.PacketOutgoingFooter;
            OutgoingFooterLength = (Int16) cfg.PacketOutgoingFooter.Length;
            IncomingFooter = cfg.PacketIncomingFooter;
            IncomingFooterLength = (Int16) cfg.PacketIncomingFooter.Length;
            EndPoint = cfg.IpEndPoint;

            NetFw = new NetFwHandler(cfg.FirewallSeconds, cfg.FirewallMaxConnections);

            SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DontFragment, cfg.NoFragment);
            SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, cfg.NoNagel);
            SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, cfg.KeepAlive);
            SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, cfg.GracefulDisconnect);
            SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, cfg.ReuseEndpoint);

            Bind(EndPoint);
            Listen(cfg.Backlog);

            Accept(SocketAsyncEventArgsPool.Acquire(OnAccept));
        }

        public void Stop()
        {
            if (IsBound)
                Shutdown(SocketShutdown.Both);
            Close();
        }


        private void Accept(SocketAsyncEventArgs accEa)
        {
            try
            {
                accEa.AcceptSocket = null;
                bool Event = AcceptAsync(accEa);
                if (!Event)
                    OnAccept(null, accEa);
            }

            catch (Exception e)
            {
                SocketAsyncEventArgs oldEa = accEa;
                oldEa.Completed -= OnAccept;
                Accept(SocketAsyncEventArgsPool.Acquire(OnAccept));
                oldEa.Release();
                ExceptionManager.RegisterException(e);
            }
        }

        private void OnAccept(object o, SocketAsyncEventArgs a)
        {
            Socket sock = a.AcceptSocket;
            if (!sock.Connected) return;

            try
            {
                if (!NetFw.PassFwPolicy(((IPEndPoint) sock.RemoteEndPoint).Address))
                {
                    NetFw.AddRule(((IPEndPoint) sock.RemoteEndPoint).Address, ((IPEndPoint) LocalEndPoint).Port);
                    sock.Terminate();
                    return;
                }

                EventHandler<ConnectionEventArgs> passConnectEvent = ClientConnected;
                if (passConnectEvent != null)
                    passConnectEvent(this, new ConnectionEventArgs(sock, _propagator));
            }
            catch (Exception ex)
            {
                ExceptionManager.RegisterException(ex);
            }
            Accept(a);
        }
    }
}