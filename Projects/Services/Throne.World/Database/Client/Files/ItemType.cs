using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Throne.World.Database.Client.Files;
using Throne.World.Structures.Objects;

namespace Throne.World.Database.Client
{
    [StructLayout(LayoutKind.Sequential)]
    public class ItemTemplate : IClientDbRecord
    {
        public Int32 Id { get; set; }
        public List<ItemCompositionBonus> CompositionBonuses { get; private set; }
        public Dictionary<Byte, ItemTemplate> RelativeItemTemplates { get; private set; }

        public ItemTemplate()
        {
            RelativeItemTemplates = new Dictionary<Byte, ItemTemplate>();
        }

        public void GetCompositionBonuses()
        {
            ItemCompositionBonus data;
            if (ItemManager.ItemCompositionBonuses.TryGetValue(
                Id/100000 == 4 || Id/100000 == 5 // Math by BaussHacker
                    ? (Id/100000)*111000 + (Id/10*10%1000)
                    : Id/10*10,
                out data))
                CompositionBonuses = data.Levels;
        }

        public readonly String Name;
        public readonly Role.Profession RequiredProfession;
        public readonly Int32 RequiredWeaponSkill;
        public readonly Byte RequiredLevel;
        public readonly Model.SexType RequiredSex;



        public readonly UInt16
            RequiredStrength,
            RequiredAgility,
            RequiredVitality,
            RequiredSpirit;

        public readonly Int32
            Monopoly,
            Weight,
            MoneyPrice,
            ActionId,
            MaxAttack,
            MinAttack,
            Defense,
            Dexterity,
            Dodge,
            Life,
            Mana,
            Amount,
            AmountLimit,
            Ident,
            FirstSocket,
            SecondSocket,
            Magic1,
            Magic2,
            Magic3,
            Data,
            MagicAttack,
            MagicDefense,
            AttackRange,
            AttackSpeed,
            FrayMode,
            RepairMode,
            TypeMask,
            EMoneyPrice,
            EMoneyMonopolyPrice,
            SaveTime,
            CriticalRate,
            MagicCriticalRate,
            AntiCriticalRate,
            MagicPenetration,
            ShieldBlock,
            CrashAttack,
            StableDefense,
            AccumulateLimit,
            Metal,
            Wood,
            Water,
            Fire,
            Earth;

        public readonly String TypeDescription, Description;

        public readonly Int32 NameColor;

        public readonly Byte SoulLevel;

        public readonly Int32 MeteorCount, RecoverEnergy, ItemClass, AuctionDeposit;

        public Int32 Type
        {
            get { return Id/1000; }
        }

        public Boolean IsEquipment
        {
            get
            {
                return ItemClass >= ItemManager.ItemClassIds["Weapon"] &&
                       ItemClass <= ItemManager.ItemClassIds["Garment/Accessory"];
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}