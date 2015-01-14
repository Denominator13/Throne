using Throne.World.Network.Messages;
using Throne.World.Scripting.Scripts;
using Throne.World.Structures.Travel;

public sealed class MailManager : NpcScript
{
    protected override void Load()
    {
        SetLongName("Mail Manager");
        SetIdentity(1651);
        SetFacing(Orientation.Southeast);
        SetFace(58);
        SetMesh(1651);
        SetLocation(new Location(1002, new Position(295, 234)));
        SetType(NpcInformation.Types.Talker);
    }

    public override void Interact()
    {
        Character.User.Send(new GeneralAction(ActionType.OpenWindow, Character).OpenWindow(GeneralAction.WindowId.MailBox));
    }
}