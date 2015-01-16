using System;
using System.IO;
using JetBrains.Annotations;

namespace Throne.Framework.Logging
{
    /// <summary>
    ///     A "proxy" to be instantiated in classes for named-source logging calls.
    /// </summary>
    public sealed class Logger
    {
        private string _logFile;

        public Logger(string name)
        {
            Name = name;
            LogFile = "log\\{0}\\{1}.txt".Interpolate(Name.Replace(":", ""), AppDomain.CurrentDomain.FriendlyName);
        }

        public string LogFile
        {
            get { return _logFile; }
            set
            {
                if (value != null)
                {
                    string pathToFile = Path.GetDirectoryName(value);

                    if (!Directory.Exists(pathToFile))
                        Directory.CreateDirectory(pathToFile);

                    if (File.Exists(value))
                    {
                        if (LogManager.ArchiveLogFiles)
                        {
                            DateTime time = File.GetLastWriteTime(value);
                            string archive = Path.Combine(pathToFile,
                                time.ToString("yyyy-MM-dd hh.mm ") + Path.GetFileName(value));

                            if (File.Exists(archive))
                                File.Delete(archive);

                            File.Move(value, archive);
                        }
                        else
                            File.Delete(value);
                    }
                }

                _logFile = value;
            }
        }

        public string Name { get; private set; }

        [StringFormatMethod("format")]
        public void Info(string format, params object[] args)
        {
            LogManager.WriteLine(LogType.Info, this, format, args);
        }

        [StringFormatMethod("format")]
        public void Status(string format, params object[] args)
        {
            LogManager.WriteLine(LogType.Status, this, format, args);
        }

        [StringFormatMethod("format")]
        public void Warn(string format, params object[] args)
        {
            LogManager.WriteLine(LogType.Warning, this, format, args);
        }

        [StringFormatMethod("format")]
        public Exception Error(string format, params object[] args)
        {
            LogManager.WriteLine(LogType.Error, this, format, args);
            return new Exception(format.Interpolate(args));
        }

        [StringFormatMethod("format")]
        public void Debug(string format, params object[] args)
        {
            LogManager.WriteLine(LogType.Debug, this, format, args);
        }

        [StringFormatMethod("format")]
        public void NotImplemented(string format, params object[] args)
        {
            LogManager.WriteLine(LogType.NotImplemented, this, format, args);
        }

        [StringFormatMethod("format")]
        public void Exception(Exception ex, string description = null, params object[] args)
        {
            if (!string.IsNullOrEmpty(description))
            {
                if (LogManager.Hide.HasFlag(LogType.Exception))
                    description += " Check the log for more information.";

                LogManager.WriteLine(LogType.Error, this, description, args);
            }

            LogManager.WriteLine(LogType.Exception, this, Environment.NewLine + ex.StackTrace, args);

            Exception innerEx = ex.InnerException;
            if (innerEx != null)
                LogManager.WriteLine(LogType.Exception, this, Environment.NewLine + ex.InnerException, args);
        }

        public void Progress(int current, int max)
        {
            LogManager.Progress(this, current, max);
        }

        public static implicit operator Boolean(Logger log)
        {
            return log != null;
        }
    }
}