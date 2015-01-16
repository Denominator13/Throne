using System;
using Throne.Framework.Logging;

namespace Throne.Framework.Utilities
{
    /// <summary>
    ///     Functions for the command line interface.
    /// </summary>
    public static class Cli
    {
        public static void WriteHeader(ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(@"       `              `  ________        `          `              `           `");
            Console.Write(@"                 `      /_  __/ /_ `_________  ____  ___    `                   ");
            Console.Write(@"                         / / /  _ \/ ___/ _  \/ __ \/ _ \               `  _    ");
            Console.Write(@"     `            `     / / / / / / /  / /_/ / / / /  __/        `        | |   ");
            Console.Write(@"          `            /_/ /_/ /_/_/   \____/_/ /_/\___/  `       `    ___| |  `");
            Console.Write(@"  `           `                 `              `             `        (    .'   ");
            Console.Write(@"______________________________________________________ Royal Flush __  )  (  ___");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = color;
            Console.WriteLine();
            Console.Write(@"            Built from contributions with the help of God and Google            ");
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public static void Exit(Int32 exitCode, Boolean wait = true)
        {
            if (wait)
            {
                LogManager.Write(LogType.Info, "Press any key to exit.");
                Console.ReadLine();
            }
            Environment.Exit(exitCode);
        }

        public static void UpdateTitle(String text)
        {
            Console.Title = text;
        }
    }
}