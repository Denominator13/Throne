using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Throne.World.Structures.Objects;

namespace Throne.World.Structures.Storage
{
    public sealed class Inventory : ItemStorage
    {
        private readonly ConcurrentDictionary<UInt32, Item> _items;

        public Inventory(ref List<Item> payload) : base(40)
        {
            _items = new ConcurrentDictionary<uint, Item>();

            if (payload == null) return;
            foreach (var item in payload.Where(item => item.Position == Item.Positions.Inventory))
                Add(item);

            payload.RemoveAll(item => item.Position == Item.Positions.Inventory);
        }

        public override IEnumerable<Item> Items
        {
            get { return _items.Values as IReadOnlyCollection<Item>; }
        }

        public override Int32 Count
        {
            get { return _items.Count; }
        }

        public Item this[UInt32 id]
        {
            get { return _items[id]; }
            set { _items[id] = value; }
        }

        public override Boolean Add(Item item)
        {
            lock (SyncRoot)
                return _items.Count < Size && _items.TryAdd(item.ID, item);
        }

        public override Item Remove(UInt32 guid)
        {
            lock (SyncRoot)
            {
                Item ret;
                _items.TryRemove(guid, out ret);
                return ret;
            }
        }


        /// <summary>
        ///     To be used with a <see cref="T:Throne.Shared.Network.Transmission.Stream" />
        /// </summary>
        /// <param name="inv">Inventory to serialize</param>
        /// <returns></returns>
        public static implicit operator Byte[][](Inventory inv)
        {
            lock (inv.SyncRoot)
                return inv.Items.Select(item => (byte[]) item).ToArray();
        }
    }
}