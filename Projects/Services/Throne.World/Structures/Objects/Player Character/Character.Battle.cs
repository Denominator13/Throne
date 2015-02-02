using Throne.World.Structures.Battle;

namespace Throne.World.Structures.Objects
{
    public partial class Character
    {
        private Magic _magic;

        public Magic Magic
        {
            get
            {
                if (!_magic)
                    _magic = Magic.ObtainMagic(this);

                return _magic;
            }
            internal set
            {
                if (_magic == value)
                    return;
                _magic.Dispose();
                _magic = value;
            }
        }

        public override bool Attackable()
        {
            return true;
        }
    }
}