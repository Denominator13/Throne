using System;

namespace Throne.World.Structures.Objects
{
    partial class Item
    {

        public Int32 StorageSize
        {
            get { return Type == 1100003 ? 3 : Type == 1100006 ? 6 : Type == 1100009 ? 12 : 0; }
        }

        public override bool Attackable()
        {
            return false;
        }
    }
}
