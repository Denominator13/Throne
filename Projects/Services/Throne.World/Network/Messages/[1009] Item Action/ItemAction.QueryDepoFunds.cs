namespace Throne.World.Network.Messages
{
    public partial class ItemAction
    {
        private void SendDepoFund()
        {
            Argument = 1337;
            Character.User.Send(this);
        }
    }
}