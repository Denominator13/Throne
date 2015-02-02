using System;

namespace Throne.World.Structures.Objects
{
    /// <remarks>Taken from COSV3</remarks>
    public struct Hair
    {
        public int Id;

        public Hair(int id)
        {
            Id = id;
        }

        public Hair(int kind, int color)
        {
            Id = (color*100) + kind;
        }

        public int Color
        {
            get { return Id/100; }
            set { Id = (value*100) + Kind; }
        }

        public int Kind
        {
            get { return Id%100; }
            set { Id = (Color*100) + value; }
        }

        public static implicit operator Int16(Hair hair)
        {
            return (Int16) hair.Id;
        }
    }
}