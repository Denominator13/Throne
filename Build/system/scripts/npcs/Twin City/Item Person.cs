using System.Threading.Tasks;
using Throne.World;
using Throne.World.Network.Messages;
using Throne.World.Scripting.Scripts;
using Throne.World.Structures.Travel;

public sealed class ItemPerson : NpcScript
{
    protected override void Load()
    {
        SetFace(10094);
        SetMesh(290);
        SetIdentity(1337);
        SetLongName("the item giving chicken");
        SetDisplayName("Item Chicken");
        SetFacing(Orientation.South);
        SetType(NpcInformation.Types.Talker);
        SetLocation(new Location(1002, new Position(298, 273)));
    }

    protected override async Task Talk()
    {
        Message("Bawk??",
            Option("Combat suit...", 1),
            Option("Turkey run!", 2),
            Option("Artist casual", 3));

        switch ((await Response()).Option)
        {
            case 1:
                ItemManager.Instance.CreateItem(Character, 138319);
                break;
            case 2:
                Message("BAWKKK!");
                ItemManager.Instance.CreateItem(Character, 200490);
                break;
            case 3:
                ItemManager.Instance.CreateItem(Character, 193105);
                break;
        }
    }
}