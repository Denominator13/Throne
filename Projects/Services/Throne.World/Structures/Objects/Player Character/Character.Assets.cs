using System;
using System.Collections.Generic;
using System.Linq;
using Throne.Framework;
using Throne.Framework.Network.Transmission.Stream;
using Throne.World.Network.Messages;
using Throne.World.Properties;
using Throne.World.Structures.Storage;

namespace Throne.World.Structures.Objects
{
    partial class Character
    {
        public Stream ItemStream
        {
            get { return new Stream().Join((Byte[][]) _inventory).Join((Byte[][]) _gear); }
        }

        /// TODO: If no space to add a new item in inventory, send to mailbox. (quest rewards and such)
        public void AddItem(Item itm)
        {
            itm.OwnerInfo = Record;
            MoveToInventory(itm);
            User.Send(itm);
        }

        public Item RemoveItem(Item itm)
        {
            itm.OwnerInfo = null;

            if (itm.Position > Item.Positions.Inventory)
                UnequipGearSlot(GetGearSlot(itm.Position));
            return MoveFromInventory(itm.ID);
        }

        public Item GetItem(UInt32 guid)
        {
            return GetInventoryItem(guid) ?? GetGearItem(guid);
        }

        #region Currency

        public UInt32 Money
        {
            get { return Record.Money; }
            set
            {
                Record.Money = value;
                Record.Update();
            }
        }

        public uint EMoney
        {
            get { return Record.EMoney; }
            set
            {
                Record.EMoney = value;
                Record.Update();
            }
        }

        #endregion Currency

        #region Inventory

        private readonly Inventory _inventory;

        public void MoveToInventory(Item itm)
        {
            itm.Position = Item.Positions.Inventory;
            _inventory[itm.ID] = itm;
        }

        public Item MoveFromInventory(UInt32 guid)
        {
            using (ItemAction pkt = new ItemAction().Remove(guid))
                User.Send(pkt);

            return _inventory.Remove(guid);
        }

        public Item GetInventoryItem(UInt32 guid)
        {
            Item item = _inventory[guid];
            if (!item)
                User.Send(StrRes.CMSG_InventoryNoItem);
            return item;
        }

        public Boolean AdequateInventorySpace(Int32 forCount = 1)
        {
            if (_inventory.AdequateSpace(forCount))
                return true;

            User.Send(StrRes.CMSG_NotEnoughInventorySpace.Interpolate(forCount));
            return false;
        }

        #endregion Inventory

        #region Gear

        private readonly Gear _gear;

        public GearSlot GetGearSlot(Item.Positions pos)
        {
            if (pos == Item.Positions.Inventory)
                throw new ArgumentException("{0} is not part of the gear collection.".Interpolate(pos));
            return _gear[pos];
        }

        public Int32 GetGearType(Item.Positions pos)
        {
            return GetGearSlot(pos).ContainedType;
        }

        public UInt32 GetGearGuid(Item.Positions pos)
        {
            return GetGearSlot(pos).ContainedGuid;
        }

        public Item GetGearItem(UInt32 guid)
        {
            GearSlot containingSlot = _gear.Values.SingleOrDefault(slot => slot.ContainedGuid == guid);
            return containingSlot != null ? containingSlot.Item : null;
        }

        public void EquipGearSlot(Item item, Item.Positions pos)
        {
            GearSlot slot = GetGearSlot(pos);
            if (!slot.Empty) UnequipGearSlot(slot);
            if (!slot.Equip(item)) return;

            item.Position = pos;
            User.Send(new ItemAction().Equip(item));
            User.Send(new ItemAction().SendGear(this));


            switch (pos)
            {
                case Item.Positions.Headgear:
                case Item.Positions.Armor:
                case Item.Positions.RightHand:
                case Item.Positions.LeftHand:
                case Item.Positions.Garment:
                case Item.Positions.Mount:
                case Item.Positions.MountArmor:
                case Item.Positions.RightWeaponAccessory:
                case Item.Positions.LeftWeaponAccessory:
                    SendToLocal();
                    break;
            }
        }

        public void UnequipGearSlot(GearSlot slot)
        {
            Item item = slot.Unequip();
            if (item)
            {
                MoveToInventory(item);
                User.Send(new ItemAction().Unequip(slot.Slot, item.ID));
                User.Send(new ItemAction().SendGear(this));
            }

            switch (slot.Slot)
            {
                case Item.Positions.Headgear:
                case Item.Positions.Armor:
                case Item.Positions.RightHand:
                case Item.Positions.LeftHand:
                case Item.Positions.Garment:
                case Item.Positions.Mount:
                case Item.Positions.MountArmor:
                case Item.Positions.RightWeaponAccessory:
                case Item.Positions.LeftWeaponAccessory:
                    SendToLocal();
                    break;
            }
        }

        #endregion Gear

        #region Depositories

        public Dictionary<DepositoryType, Dictionary<DepositoryId, ItemDepository>> Depositories;

        private void ConstructItemDepositories(ref List<Item> payload)
        {
            Depositories = new Dictionary<DepositoryType, Dictionary<DepositoryId, ItemDepository>>();

            Depositories[DepositoryType.Sash] = new Dictionary<DepositoryId, ItemDepository>();
            Depositories[DepositoryType.Warehouse] = new Dictionary<DepositoryId, ItemDepository>();
            Depositories[DepositoryType.Chest] = new Dictionary<DepositoryId, ItemDepository>();

            foreach (DepositoryId warehouse in
                Enum.GetValues(typeof (DepositoryId))
                    .Cast<DepositoryId>()
                    .Where(wareId => wareId != DepositoryId.None))
                Depositories[DepositoryType.Warehouse].Add(warehouse, new ItemDepository(100));

            foreach (Item sash in payload.Where(item => item.StorageSize > 0))
                Depositories[DepositoryType.Sash].Add((DepositoryId) sash.ID, new ItemDepository(sash.StorageSize));

            foreach (Item item in payload.Where(item => item.DepositoryType != DepositoryType.None))
                Depositories[item.DepositoryType][item.DepositoryId].Add(item);

            payload.RemoveAll(
                remove => remove.DepositoryType != DepositoryType.None);
        }

        public void MoveToDepository(DepositoryType type, DepositoryId id, Item item)
        {
            ItemDepository depo;
            if (!Depositories[type].TryGetValue(id, out depo)) return;

            lock (depo.SyncRoot)
            {
                if (!depo.AdequateSpace()) return;
                if (item.Position > Item.Positions.Inventory)
                    UnequipGearSlot(GetGearSlot(item.Position));
                depo.Add(MoveFromInventory(item.ID));

                item.DepositoryId = id;
                item.DepositoryType = type;

                User.Send(new DepositoryInformation(type, id, DepositoryInformation.DepositoryAction.ShowItem, 1, item));
            }
        }

        public Boolean MoveFromDepository(DepositoryType type, DepositoryId id, UInt32 itemId)
        {
            ItemDepository depo;
            if (!Depositories[type].TryGetValue(id, out depo)) return false;

            lock (depo.SyncRoot)
            {
                if (!AdequateInventorySpace()) return false;
                Item item = depo.Remove(itemId);
                item.DepositoryType = DepositoryType.None;
                User.Send(item);
                MoveToInventory(item);
                return true;
            }
        }

        #endregion
    }
}