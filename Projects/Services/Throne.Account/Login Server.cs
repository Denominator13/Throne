using System;
using System.Net;
using Throne.Framework.Cryptography;
using Throne.Framework.Network;
using Throne.Framework.Network.Communication;
using Throne.Framework.Network.Connectivity;
using Throne.Framework.Security.Permissions;
using Throne.Framework.Services;
using Throne.Framework.Services.Account;
using Throne.Framework.Utilities;
using Throne.Login.Annotations;
using Throne.Login.Configuration;
using Throne.Login.Network;
using Throne.Login.Network.Handling;
using Throne.Login.Network.Messages;
using Throne.Login.Properties;
using Throne.Login.Records.Implementations;
using Throne.Login.Services;

namespace Throne.Login
{
    [UsedImplicitly]
    internal sealed class LoginServer : NetworkApplication<LoginServer>
    {
        #region Services

        public AccountDatabaseContext AccountDbContext { get; private set; }

        #endregion

        private ServiceHost<IAccountService, AccountService> _accountHost;

        private LoginServer()
        {
        }

        public static LoginConfiguration Configuration { get; private set; }

        public override IPEndPoint EndPoint
        {
            get { return Configuration.Network.IpEndPoint; }
        }

        protected override void OnStart(string[] args)
        {
            Cli.UpdateTitle("Starting " + ToString());
            Cli.WriteHeader(ConsoleColor.Cyan);

            LoadConfiguration(Configuration = new LoginConfiguration());

            InitiatePersistence(AccountDbContext = new AccountDatabaseContext(), Configuration.Persistence);

            StartAccountService();

            Log.Status("Configuring TCP server on {0}.", EndPoint);
            Server.Start(new AuthenticationPacketPropagator(), Configuration.Network);
            Log.Status("Ready for connections.");

            Cli.UpdateTitle(ToString());
        }

        public void StartAccountService()
        {
            Log.Status(StrRes.SMSG_IPCDeviceStart, "account");

            try
            {
                _accountHost = new ServiceHost<IAccountService, AccountService>(new AccountService(),
                    Configuration.Services.AccountServiceUri);

                _accountHost.Open();
            }

            catch (Exception ex)
            {
                Log.Exception(ex, "Unable to start account service. {0}", ex.Message);
                Cli.Exit(1);
            }

            Log.Status(StrRes.SMSG_IPCDeviceStarted, "account");
        }


        protected override void OnStop()
        {
            if (_accountHost != null)
                _accountHost.Close();
            base.OnStop();
        }

        protected override TcpServer CreateServer()
        {
            return new TcpServer();
        }

        protected override void OnClientConnected(object sender, ConnectionEventArgs args)
        {
            args.StreamCipher = new NetDragonAuthenticationCipher();

            var authClient = new LoginClient(args);

            authClient.AddPermission(new ConnectedPermission());

            using (var packet = new AuthCipherSeed(authClient.SocketId))
                authClient.Send(packet);
        }

        protected override void OnClientDisconnected(object sender, ConnectionEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void UpdateTitle()
        {
            Console.Title = ToString();
        }
    }
}