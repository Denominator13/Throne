using System;
using System.Globalization;
using System.IO;

namespace Throne.Framework.Logging
{
    [Flags]
    public enum LogType
    {
        Info = 0x1,
        Warning = 0x2,
        Error = 0x4,
        Debug = 0x8,
        Status = 0x10,
        Exception = 0x20,
        NotImplemented = 0x40,
        Packet = 0x80,
        None = 0x100
    }

    /// <summary>
    ///     Takes care of command line and file logging in the entire source base.
    ///     Do not use this class directly (except for debug logging); use LogProxy objects instead.
    /// </summary>
    public static class LogManager
    {
        private static readonly object Sync = new object();

        public static Boolean ClTimestamps { get; set; }
        public static Boolean ClColorize { get; set; }
        public static Boolean FileLogging { get; set; }
        public static Boolean ArchiveLogFiles { get; set; }
        public static Boolean LogPackets { get; set; }

        public static LogType Hide { get; set; }

        public static void Progress(Logger source, int current, int max)
        {
            float donePercent = (100f/max*current);
            var done = (int) System.Math.Ceiling(20f/max*current);

            Write(LogType.Status, source, false,
                "[" + ("".PadRight(done, '#') + "".PadLeft(20 - done, '.')) + "] {0,5}%\r",
                donePercent.ToString("0.0", CultureInfo.InvariantCulture));
            if (donePercent == 100.00)
                WriteLine();
        }

        public static void WriteLine(LogType level, Logger source, string format, params object[] args)
        {
            Write(level, source, format + Environment.NewLine, args);
        }

        public static void WriteLine()
        {
            WriteLine(LogType.None, null, "");
        }

        public static void Write(LogType level, string format, params object[] args)
        {
            Write(level, null, true, format, args);
        }

        public static void Write(LogType level, Logger source, string format, params object[] args)
        {
            Write(level, source, true, format, args);
        }

        private static void Write(LogType level, Logger source, bool toFile, string format, params object[] args)
        {
            lock (Sync)
            {
                if (!Hide.HasFlag(level))
                {
                    if (ClTimestamps && toFile)
                        Console.Write(DateTime.Now.ToString("T"));

                    if (ClColorize)
                        switch (level)
                        {
                            case LogType.Info:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case LogType.Warning:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case LogType.Error:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case LogType.Debug:
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            case LogType.Status:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case LogType.Exception:
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                break;
                            case LogType.NotImplemented:
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                break;
                            case LogType.Packet:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                        }

                   
                        if (source)
                            Console.Write("[{0}]", source.Name);
                        else if (level != LogType.None)
                            Console.Write("[{0}]", level);

                        Console.ForegroundColor = ConsoleColor.Gray;

                        if (level != LogType.None)
                            Console.Write(" ");
                    try
                    {
                        Console.Write(format, args);
                    }
                    catch (FormatException)
                    {
                        Console.Write(format);
                    }
                }

                if (!source || !toFile || !FileLogging) return;
                if (string.IsNullOrEmpty(source.LogFile)) return;
                using (var file = new StreamWriter(source.LogFile, true))
                {
                    file.Write(DateTime.Now + " ");
                    if (level != LogType.None)
                        file.Write("[{0}] - ", level);
                    try
                    {
                        file.Write(format, args);
                    }
                    catch (FormatException)
                    {
                        file.Write(format);
                    }
                    file.Flush();
                }
            }
        }
    }
}