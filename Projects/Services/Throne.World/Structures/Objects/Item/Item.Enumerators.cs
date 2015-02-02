namespace Throne.World.Structures.Objects
{
    partial class Item
    {
        public enum Color
        {
            Black = 2,
            Orange = 3,
            LightBlue = 4,
            Red = 5,
            Blue = 6,
            Yellow = 7,
            Purple = 8,
            White = 9
        }

        public enum EquipmentQuality
        {
            Fixed = 0,
            Normal = 5,
            Refined = 6,
            Unique = 7,
            Elite = 8,
            Super = 9
        }

        public enum ActiveEffect
        {
            None = 0,
            Steed = 100,
            Poison = 200,
            HP = 201,
            MP = 202,
            Shield = 203
        }

        public enum Positions
        {
            Inventory = 0,

            //has alts
            Headgear = 1,
            Lavalier = 2,
            Armor = 3,
            RightHand = 4,
            LeftHand = 5,
            Band = 6,
            Talisman = 7,
            Boots = 8,
            Garment = 9,

            //no alts
            Fan = 10,
            Tower = 11,
            Mount = 12,
            RightWeaponAccessory = 15,
            LeftWeaponAccessory = 16,
            MountArmor = 17,
            MountTalisman = 18,

            AlternateHeadgear = 21,
            AlternateLavalier = 22,
            AlternateArmor = 23,
            AlternateRightHand = 24,
            AlternateLeftHand = 25,
            AlternateBand = 26,
            AlternateTalisman = 27,
            AlternateBoots = 28,
            AlternateGarment = 29
        }
    }
}