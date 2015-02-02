using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Throne.Framework.Utilities;

namespace Throne.World.Database.Client.Files
{
    public static class ItemAdd
    {
        private const String Path = "system/database/itemadd.ini";
        private static readonly FieldInfo[] Fields = typeof (ItemCompositionBonus).GetFields();

        public static void Read(out Dictionary<Int32, ItemCompositionBonus> bonuses)
        {
            bonuses = new Dictionary<Int32, ItemCompositionBonus>();
            using (var reader = new FileReader(Path))
                foreach (ItemCompositionBonus record in
                    reader.Select(line => ClientDatabaseReader.CreateRecord<ItemCompositionBonus>(line, Fields, " ")))
                    if (bonuses.ContainsKey(record.Id))
                        bonuses[record.Id].Levels.Add(record);
                    else
                    {
                        record.Levels.Add(record);
                        bonuses[record.Id] = record;
                    }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class ItemCompositionBonus : IClientDbRecord
    {
        public List<ItemCompositionBonus> Levels { get; set; }

        public ItemCompositionBonus()
        {
            Levels = new List<ItemCompositionBonus>();
        }

        public int Id { get; set; }

        public readonly Int32 Level, MinAttack, MaxAttack, MagicAttack, Defense, Dodge, MagicDefense, Health, Dexterity;
    }
}