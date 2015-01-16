using Throne.Framework.Logging;

namespace Throne.Framework.Configuration.Files
{
    public sealed class LoggingConfigFile : ConfigurationFile
    {
        public void Load()
        {
            Require("system/config/Logging.cfg");

            LogManager.Hide = GetEnum<LogType>("hide_log_levels");
            LogManager.ClColorize = GetBool("cl_colorize", true);
            LogManager.ClTimestamps = GetBool("cl_timestamps");
            LogManager.FileLogging = GetBool("file_logging", true);
            LogManager.ArchiveLogFiles = GetBool("archive_log_files", true);
            LogManager.LogPackets = GetBool("log_incoming_packets");
        }
    }
}