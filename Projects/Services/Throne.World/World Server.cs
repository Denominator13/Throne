using System;
using System.Net;
using Throne.Framework;
using Throne.Framework.Cryptography;
using Throne.Framework.Network;
using Throne.Framework.Network.Communication;
using Throne.Framework.Network.Connectivity;
using Throne.Framework.Security.Permissions;
using Throne.Framework.Services;
using Throne.Framework.Services.Account;
using Throne.Framework.Utilities;
using Throne.World.Configuration;
using Throne.World.Database.Records.Implementations;
using Throne.World.Managers;
using Throne.World.Network;
using Throne.World.Network.Handling;
using Throne.World.Properties;
using Throne.World.Records;

namespace Throne.World
{
    internal sealed class WorldServer : NetworkApplication<WorldServer>
    {
        private WorldServer()
        {
            Events = new EventManager();
        }

        public static WorldConfiguration Configuration { get; private set; }
        public EventManager Events { get; private set; }
        public ServerInfoRecord Info { get; private set; }
        public WorldDatabaseContext WorldDbContext { get; private set; }
        public IpcDevice<IAccountService, EmptyCallbackService> AccountService { get; private set; }

        public override IPEndPoint EndPoint
        {
            get { return new IPEndPoint(Configuration.Network.Host, Configuration.Network.Port); }
        }

        protected override void OnClientConnected(object sender, ConnectionEventArgs args)
        {
            args.StreamCipher = new GameCipher(Configuration.Network.Cast5Standard);

            var worldClient = new WorldClient(args);
            worldClient.AddPermission(new ConnectedPermission());
            Info.OnlineCount++;
            Info.Update();
        }

        protected override void OnClientDisconnected(object sender, ConnectionEventArgs args)
        {
            Info.OnlineCount--;
            Info.Update();
        }

        protected override void OnStart(string[] args)
        {
            Cli.UpdateTitle("Starting " + ToString());
            Cli.WriteHeader(ConsoleColor.Red);

            LoadConfiguration(Configuration = new WorldConfiguration());

            InitiatePersistence(WorldDbContext = new WorldDatabaseContext(), Configuration.Persistence);

            Info = ServerInfoManager.Instance.Get(WorldConfiguration.ServerName);
            LinkAccountService();

            ItemManager.Instance.Load();
            BattleManager.Instance.Load();
            ScriptManager.Instance.Load();

            Log.Status("Configuring TCP server on {0}.", EndPoint);
            Server.Start(new WorldPacketPropagator(), Configuration.Network);
            Log.Status("Ready for connections.");

            Cli.UpdateTitle(ToString());
        }

        private void LinkAccountService()
        {
            Log.Status(StrRes.SMSG_IPCDeviceConnect, "account");

            try
            {
                AccountService = new IpcDevice<IAccountService, EmptyCallbackService>(() =>
                    new DuplexServiceClient<IAccountService, EmptyCallbackService>(new EmptyCallbackService(),
                        Configuration.Services.AccountServiceUri));

                AccountService.Connect();
            }

            catch (Exception ex)
            {
                Log.Exception(ex, "Unable to contact account service. {0}", ex.Message);
                Cli.Exit(1);
            }

            Log.Status(StrRes.SMSG_IPCDeviceConnected, "account");
        }

        protected override void OnStop()
        {
            if (AccountService != null)
                AccountService.Dispose();
            base.OnStop();
        }

        protected override TcpServer CreateServer()
        {
            return new TcpServer();
        }

        protected override void Pulse(TimeSpan diff)
        {
        }

        public override string ToString()
        {
            return "{0} - {1}".Interpolate(WorldConfiguration.ServerName, base.ToString());
        }
    }
}