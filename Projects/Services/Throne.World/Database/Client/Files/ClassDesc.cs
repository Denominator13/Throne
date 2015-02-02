using System;
using System.Collections.Generic;
using Throne.Framework.Utilities;

namespace Throne.World.Database.Client.Files
{
    /// <summary>
    ///     Serial number for each item type. Also used for the auction interface.
    /// </summary>
    public static class ClassDesc
    {
        public const String Path = "system/database/ClassDesc.ini";

        public static void Read(out Dictionary<String, Int32> dict)
        {
            dict = new Dictionary<String, Int32>();
            using (var reader = new FileReader(Path))
                foreach (string line in reader)
                {
                    string[] data = line.Split('#');
                    int num = Convert.ToInt32(data[0]);
                    dict[data[1]] = num < 1000 ? num*1000 : num;
                }
        }
    }
}