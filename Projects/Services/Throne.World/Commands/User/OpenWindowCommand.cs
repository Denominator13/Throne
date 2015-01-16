using System;
using Throne.Framework.Commands;
using Throne.World.Network.Messages;

namespace Throne.World.Commands.User
{
    [Command("OpenWindow", "Window")]
    public class OpenWindowCommand : UserCommand
    {
        public override string Description
        {
            get { return "Opens a window for the user."; }
        }

        public override void ExecuteUserCommand()
        {
            var windowId = Arguments.NextEnum<GeneralAction.WindowId>();

            Target.User.Send(new GeneralAction(ActionType.OpenWindow, Target).OpenWindow(windowId));
        }
    }
}