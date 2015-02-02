using Throne.World.Structures.Travel;

namespace Throne.World.Structures.Objects
{
    public abstract partial class Role : WorldObject
    {
        protected Hair _hair;
        protected Model _look;

        public Role(uint ID)
            : base(ID)
        {
        }

        public virtual Model Look
        {
            get { return _look; }
            set { _look = value; }
        }

        public virtual Hair Hairstyle
        {
            get { return _hair; }
            set { _hair = value; }
        }

        public abstract void ExchangeSpawns(Character with);

        /// <summary>
        ///     Sends this entity's spawn at the edge of the other character's screen, then sends a jump.
        /// </summary>
        /// <param name="with"></param>
        /// <param name="jmp"></param>
        /// <remarks>
        ///     TODO:
        ///     I'm clearly rushing here. My method for entering into screens gracefully is not proper.
        ///     The position of this entity is relocated, causing other players to exchange invalid spawn info or
        ///     to interact with the player at the old location.
        /// </remarks>
        public virtual void ExchangeAerialSpawns(Character with, Jump jmp)
        {
            Position pos = Location.Position.GetPrevious();
            Position otherPos = with.Location.Position;
            int pDistance = otherPos - pos;
            int gap = WorldServer.Configuration.World.PlayerScreenRange - pDistance;
            Position fauxPos = otherPos.GetRelative(pos, gap);

            Location.Position.Relocate(fauxPos);
            ExchangeSpawns(with);
            Location.Position.Restore();

            with.User.Send(jmp.Info);
        }
    }
}