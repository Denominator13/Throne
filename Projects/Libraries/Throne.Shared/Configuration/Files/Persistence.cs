using System;
using Throne.Framework.Persistence;

namespace Throne.Framework.Configuration.Files
{
    public class PersistenceConfigFile : ConfigurationFile
    {
        public string Database;
        public DatabaseType DatabaseType;
        public string Host;
        public string Password;
        public string Username;

        public String ConnectionString
        {
            get
            {
                return "Server={0};Database={1};User ID={2};Password={3};Allow Zero Datetime=True"
                    .Interpolate(Host, Database, Username, Password);
            }
        }

        public void Load()
        {
            Require("system/config/Persistence.cfg");

            DatabaseType = GetEnum("database_type", DatabaseType.MySql);
            Host = GetString("host", "localhost");
            Username = GetString("username", "root");
            Password = GetString("password");
            Database = GetString("database", "throne");
        }
    }
}