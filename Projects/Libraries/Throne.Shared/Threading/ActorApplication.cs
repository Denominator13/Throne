using System;
using System.Reflection;
using System.Threading;
using Throne.Framework.Configuration;
using Throne.Framework.Configuration.Files;
using Throne.Framework.Exceptions;
using Throne.Framework.Logging;
using Throne.Framework.Persistence;
using Throne.Framework.Threading.Actors;
using Throne.Framework.Utilities;

namespace Throne.Framework.Threading
{
    public abstract class ActorApplication<T> : SingletonActor<T>
        where T : ActorApplication<T>
    {
        public const int UpdateDelay = 20000;

        protected readonly ActorTimer _updateTimer;
        public Logger Log;

        private DateTime _lastUpdate;

        private bool _shouldStop;

        protected ActorApplication()
        {
            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;

            Log = new Logger(GetType().Name);

            _updateTimer = new ActorTimer(this, UpdateCallback, TimeSpan.FromMilliseconds(UpdateDelay), UpdateDelay);
            _lastUpdate = DateTime.Now;
        }

        private static string AssemblyVersion
        {
            get
            {
                Version ver = Assembly.GetEntryAssembly().GetName().Version;
                return "{0}.{1}.{2}.{3}".Interpolate(ver.Major, ver.Minor, ver.Build, ver.Revision);
            }
        }

        public event EventHandler Shutdown;

        public void LoadConfiguration(BaseConfiguration cfg)
        {
            try
            {
                cfg.Load();
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Unable to read configuration. {0}", ex.Message);
                Cli.Exit(1);
            }

            cfg.Log.Status("Loaded.");
        }

        public void InitiatePersistence(DatabaseContext ctx, PersistenceConfigFile cfg)
        {
            try
            {
                ctx.Log.Status("Configuring...");
                ctx.Configure(cfg);
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Unable to configure database. {0}", ex.Message);
                Cli.Exit(1);
            }

            ctx.Log.Status("Ready.");
        }

        protected override void Dispose(bool disposing)
        {
            _updateTimer.Dispose();

            base.Dispose(disposing);
        }

        private void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ex = (Exception)e.ExceptionObject;
                Log.Exception(ex, "Unhandled Exception: {0}", ex.Message);
                Log.Status("Closing server.");
                Stop();
            }
            catch
            {
                try{Log.Error("Failed to close the server properly!");}
                catch{}
            }
            Cli.Exit(1);
        }

        public virtual void Start(string[] args)
        {
            try
            {
                OnStart(args);
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "The server could not be started. {0}", ex.Message);
            }
            GC.Collect(2, GCCollectionMode.Optimized);
        }

        public void Stop()
        {
            try
            {
                EventHandler shutdownEvent = Shutdown;
                if (shutdownEvent != null)
                    shutdownEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                ExceptionManager.RegisterException(ex);
            }

            try
            {
                OnStop();
            }
            catch (Exception ex)
            {
                ExceptionManager.RegisterException(ex);
            }

            GC.Collect();

            _shouldStop = true;
        }

        protected virtual void OnStart(string[] args)
        {
        }

        protected virtual void OnStop()
        {
        }

        private void UpdateCallback()
        {
            if (_shouldStop)
            {
                _updateTimer.Change(Timeout.InfiniteTimeSpan);
                return;
            }

            DateTime now = DateTime.Now;
            TimeSpan diff = now - _lastUpdate;
            _lastUpdate = now;

            Pulse(diff);
        }

        protected virtual void Pulse(TimeSpan diff)
        {
        }

        public override string ToString()
        {
            return "Throne {0} (v{1})".Interpolate(GetType().Name, AssemblyVersion);
        }
    }
}