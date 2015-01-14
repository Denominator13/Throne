using System;
using System.Collections.Generic;
using System.Threading;
using Throne.World.Structures.Objects;

namespace Throne.World.Structures.Storage
{
    public abstract class ItemStorage
    {
        private Object _syncRoot;
        public readonly Int32 Size;

        public ItemStorage(Int32 size)
        {
            Size = size;
        }

        public Object SyncRoot
        {
            get
            {
                if (_syncRoot == null) Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);
                return _syncRoot;
            }
        }

        public Boolean AdequateSpace(Int32 forCount = 1)
        {
            return Count + forCount <= Size;
        }

        public abstract IEnumerable<Item> Items { get; }

        /// <summary>
        ///     Returns amount of items in this storage.
        /// </summary>
        public abstract Int32 Count { get; }

        /// <summary>
        ///     Adds item to storage.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True if the item could be added to this storage.</returns>
        public abstract Boolean Add(Item item);

        /// <summary>
        ///     Removes item from storage.
        /// </summary>
        /// <param name="guid">Item ID.</param>
        /// <returns>The item removed.</returns>
        public abstract Item Remove(UInt32 guid);
    }
}