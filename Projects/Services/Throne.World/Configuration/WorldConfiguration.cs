using System;
using Throne.Framework.Configuration;
using Throne.World.Configuration.Files;

namespace Throne.World.Configuration
{
    public sealed class WorldConfiguration : BaseConfiguration
    {
        public static String ServerName =
            AppDomain.CurrentDomain.FriendlyName
                .Replace(".exe", "")
                .Replace("Throne.", "");

        internal WorldConfiguration()
        {
            Network = new NetworkConfigFile();
            World = new WorldConfigFile();
        }

        /// <summary>
        ///     ServerName/Network.cfg
        /// </summary>
        public NetworkConfigFile Network { get; private set; }

        /// <summary>
        ///     ServerName/World.cfg
        /// </summary>
        public WorldConfigFile World { get; set; }

        public override void Load()
        {
            LoadDefault();

            Network.Load();
            World.Load();
        }
    }
}