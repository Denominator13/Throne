﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Throne.World.Structures.Objects;

namespace Throne.World.Structures
{
    public sealed class Gear : Dictionary<Item.Positions, GearSlot>
    {
        private Object _syncRoot;

        public Gear(ref List<Item> payload)
        {
            lock (SyncRoot)
                foreach (
                    var pos in
                        Enum.GetValues(typeof (Item.Positions))
                            .Cast<Item.Positions>()
                            .Where(pos => pos >= Item.Positions.Headgear && pos <= Item.Positions.AlternateGarment))
                    Add(pos, new GearSlot(pos, payload.SingleOrDefault(plItem => plItem.Position == pos)));

            payload.RemoveAll(
                item => item.Position >= Item.Positions.Headgear && item.Position <= Item.Positions.AlternateGarment);
        }

        public Object SyncRoot
        {
            get
            {
                if (_syncRoot == null) Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);
                return _syncRoot;
            }
        }


        public IReadOnlyCollection<Item> GetGear(Boolean alternate)
        {
            lock (SyncRoot)
                return
                    Values
                        .Where(gSlot => !gSlot.Empty && (
                            alternate
                                ? gSlot.Slot > Item.Positions.MountTalisman &&
                                  gSlot.Slot <= Item.Positions.AlternateGarment
                                : gSlot.Slot > Item.Positions.Inventory &&
                                  gSlot.Slot <= Item.Positions.Garment
                                  ||
                                  gSlot.Slot > Item.Positions.Garment &&
                                  gSlot.Slot <= Item.Positions.MountTalisman))
                        .Select(eqSlot => eqSlot.Item)
                        as IReadOnlyCollection<Item>;
        }

        public static implicit operator Byte[][](Gear gear)
        {
            return gear.Values.Where(gSlot => !gSlot.Empty).Select(gSlot => (byte[])gSlot.Item).ToArray();
        }
    }
}