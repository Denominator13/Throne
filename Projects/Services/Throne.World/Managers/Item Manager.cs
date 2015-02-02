using System;
using System.Collections.Generic;
using System.Linq;
using Throne.Framework;
using Throne.Framework.Logging;
using Throne.Framework.Threading;
using Throne.World.Database.Client;
using Throne.World.Database.Client.Files;
using Throne.World.Database.Records.Implementations;
using Throne.World.Records;
using Throne.World.Structures.Objects;

namespace Throne.World
{
    public sealed class ItemManager : SingletonActor<ItemManager>
    {
        public static Dictionary<Int32, ItemTemplate> Templates;
        public static Dictionary<Int32, ItemCompositionBonus> ItemCompositionBonuses;
        public static Dictionary<String, Int32> ItemClassIds;

        public readonly Logger Log;
        private readonly SerialGenerator _serialGenerator;

        private ItemManager()
        {
            Log = new Logger("ItemManager");

            SerialGeneratorManager.Instance.GetGenerator(typeof (ItemRecord).Name, WorldObject.ItemIdMin,
                WorldObject.ItemIdMax, ref _serialGenerator);
        }

        public void Load()
        {
            ClassDesc.Read(out ItemClassIds);
            ItemAdd.Read(out ItemCompositionBonuses);
            ClientDatabaseReader.Read("itemtype.dat", out Templates);

            Templates.Values.With(t => t.GetCompositionBonuses());

            Log.Status("Item templates loaded.");
        }

        public Item CreateItem(Character chr, Int32 type)
        {
            var record = new ItemRecord
            {
                Guid = _serialGenerator.Next(),
                Owner = chr.Record,
                Type = type
            };
            record.Create();

            var item = new Item(record);
            chr.AddItem(item);
            item.Script.OnCreation(chr, item);

            return item;
        }

        public ItemRecord CreateItemRecord(CharacterRecord cRecord, Int32 type, Byte craftLevel, Byte firstSlot,
            Byte secondSlot, Item.Positions position)
        {
            var record = new ItemRecord
            {
                Guid = _serialGenerator.Next(),
                Owner = cRecord,
                Type = type,
                CraftLevel = craftLevel,
                FirstSlot = firstSlot,
                SecondSlot = secondSlot,
                Position = position
            };
            record.Create();

            return record;
        }

        public ItemRecord CreateItemRecord(Int32 type)
        {
            var record = new ItemRecord
            {
                Guid = _serialGenerator.Next(),
                Type = type,
            };
            record.Create();

            return record;
        }
    }
}