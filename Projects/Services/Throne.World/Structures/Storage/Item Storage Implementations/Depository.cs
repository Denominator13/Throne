using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Throne.Framework;
using Throne.Framework.Network.Transmission.Stream;
using Throne.World.Network.Messages;
using Throne.World.Structures.Objects;

namespace Throne.World.Structures.Storage
{
    public class ItemDepository : ItemStorage
    {
        private readonly ConcurrentDictionary<UInt32, Item> _items;


        public ItemDepository(Int32 size)
            : base(size)
        {
            _items = new ConcurrentDictionary<uint, Item>();
        }

        public override IEnumerable<Item> Items
        {
            get { return _items.Values as IReadOnlyCollection<Item>; }
        }

        public override Int32 Count
        {
            get { return _items.Count; }
        }

        public override Boolean Add(Item item)
        {
            lock (SyncRoot)
                return _items.Count < Size && _items.TryAdd(item.ID, item);
        }

        public override Item Remove(UInt32 guid)
        {
            Item ret;
            lock (SyncRoot)
                _items.TryRemove(guid, out ret);
            return ret;
        }

        /// <summary>
        ///     Creates a server message contain depository information for the client.
        ///     May contain multiple packets in one array for depositories with large capacity.
        /// </summary>
        public Byte[] DepositoryInformationArray(DepositoryType type, DepositoryId id)
        {
            //todo: need to send refinements and artifacts.

            lock (SyncRoot)
                if (Count > 17)
                {
                    var stream = new Stream();
                    for (int joined = 0; joined < Count;)
                        Items.Skip(joined).Take(17).ToArray().With(
                            items =>
                            {
                                stream.Join(new Depository(type, id, Depository.DepositoryAction.Initiate, Size,
                                    items));
                                joined += items.Length;
                            });
                    return stream;
                }
                else
                    return new Depository(type, id, Depository.DepositoryAction.Initiate, Size, Items.ToArray());
        }
    }

    public enum DepositoryType : byte
    {
        None,
        Warehouse,
        Chest,
        Sash
    }

    public enum DepositoryId : uint
    {
        None,
        TwinCity = 8,
        PhoenixCity = 10012,
        ApeCity = 10028,
        DesertCity = 10011,
        BirdCity = 10027,
        StoneCity = 4101,
        Market = 44
    }
}