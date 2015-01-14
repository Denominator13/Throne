using Throne.World.Network.Messages;
using Throne.World.Scripting.Scripts;
using Throne.World.Structures.Travel;

public sealed class TwinCityWarehouseman : NpcScript
{
    protected override void Load()
    {
        SetLongName("Twin City warehouseman");
        SetFace(4);
        SetMesh(8);
        SetIdentity(8);
        SetFacing(Orientation.Southwest);
        SetType(NpcInformation.Types.Warehouse);
        SetLocation(new Location(1002, new Position(281, 250)));
    }

    public override void Interact()
    {
        Character.User.Send(new GeneralAction(ActionType.OpenWindow, Character).OpenWindow(GeneralAction.WindowId.Warehouse));
    }
}