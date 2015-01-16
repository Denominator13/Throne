using System;

namespace Throne.Framework.Configuration.Files
{
    public class ServicesConfigFile : ConfigurationFile
    {
        public Uri AccountServiceUri;

        public void Load()
        {
            Require("system/config/Services.cfg");

            AccountServiceUri = new Uri(GetString("account_uri"));
        }
    }
}