using Throne.Framework.Configuration.Files;

namespace Throne.Framework.Configuration
{
    public abstract class BaseConfiguration : ConfigurationFile
    {
        protected BaseConfiguration()
        {
            Logging = new LoggingConfigFile();
            Persistence = new PersistenceConfigFile();
            Services = new ServicesConfigFile();
        }

        /// <summary>
        ///     Logging.cfg
        /// </summary>
        public LoggingConfigFile Logging { get; protected set; }

        /// <summary>
        ///     Persistence.cfg
        /// </summary>
        public PersistenceConfigFile Persistence { get; protected set; }

        /// <summary>
        /// Services.cfg
        /// </summary>
        public ServicesConfigFile Services { get; protected set; }

        public void LoadDefault()
        {
            Logging.Load();
            Persistence.Load();
            Services.Load();
        }

        public abstract void Load();
    }
}