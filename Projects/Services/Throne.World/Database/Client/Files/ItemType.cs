using System;
using System.Runtime.InteropServices;
using Throne.World.Structures.Objects;

namespace Throne.World.Database.Client
{
    [StructLayout(LayoutKind.Sequential)]
    public class ItemType : IClientDatabase
    {
        public int Id { get; set; }

        public String Name;
        public Role.Profession RequiredProfession;
        public Int32 RequiredWeaponSkill;
        public Byte RequiredLevel;
        public Role.Model.SexType RequiredSex;

        public UInt16
            RequiredStrength,
            RequiredAgility,
            RequiredVitality,
            RequiredSpirit;

        public Int32
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

        public String TypeDescription, Description;

        public Item.Color ColorIndex;

        public Byte SoulLevel;

        public Int32 MeteorCount, RecoverEnergy, AuctionClass, AuctionDeposit;

        public override string ToString()
        {
            return Name;
        }
    }
}