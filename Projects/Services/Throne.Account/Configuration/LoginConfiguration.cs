using System;
using Throne.Framework.Configuration;
using Throne.Login.Configuration.Files;

namespace Throne.Login.Configuration
{
    public class LoginConfiguration : BaseConfiguration
    {
        public static String ServerName = "Login";

        public LoginConfiguration()
        {
            Network = new NetworkConfigFile();
        }

        /// <summary>
        ///     ServerName/Network.cfg
        /// </summary>
        public NetworkConfigFile Network { get; protected set; }

        public override void Load()
        {
            LoadDefault();

            Network.Load();
        }
    }
}