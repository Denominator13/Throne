using System;
using Throne.Framework;
using Throne.Framework.Configuration;

namespace Throne.World.Configuration.Files
{
    public sealed class WorldConfigFile : ConfigurationFile
    {
        public String CommandPrefix;
        public Int32 MaxJumpRange, PlayerScreenRange;

        public void Load()
        {
            Require("system/config/{0}/World.cfg".Interpolate(WorldConfiguration.ServerName));

            MaxJumpRange = GetInt("max_jump_range", 16);
            PlayerScreenRange = GetInt("player_screen_range", 18);
            CommandPrefix = GetString("command_prefix");
        }
    }
}