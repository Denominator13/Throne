using Throne.Framework.Commands;
using Throne.World.Structures.Travel;

namespace Throne.World.Commands.User.Travel
{
    [Command("Teleport", "Warp", "Move", "Loc")]
    public sealed class TeleportCommand : UserCommand
    {
        public override string Description
        {
            get { return "Teleports the user to a new location. (MapID, X, Y)"; }
        }

        public override void ExecuteUserCommand()
        {
            uint map = Arguments.NextUInt32();
            short x = Arguments.NextInt16();
            short y = Arguments.NextInt16();
            var dst = new Location(map, x, y);
            Target.Teleport(dst);
        }
    }
}