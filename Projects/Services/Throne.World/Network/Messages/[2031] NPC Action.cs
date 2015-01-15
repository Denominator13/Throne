using System;
using Throne.Framework.Network.Transmission;
using Throne.World.Network.Handling;
using Throne.World.Security;
using Throne.World.Structures.Objects;
using Throne.World.Structures.Objects.Actors;

namespace Throne.World.Network.Messages
{
    [WorldPacketHandler(PacketTypes.NpcAction)]
    public sealed class NpcAction : WorldPacket
    {
        public enum Action
        {
            Initiate,
            Add,
            Removal,
            Delete,
            ChangePosition,
            PlaceStatue
        }

        public NpcAction(Byte[] array)
            : base(array)
        {
            SeekForward(sizeof (int)); //incoming timestamp
        }

        public override bool Read(WorldClient client)
        {
            Character chr = client.Character;
            uint id = ReadUInt();
            short unknown1 = ReadShort();
            byte option = ReadByte();
            var interactionType = (Action) ReadUShort();
            string inputReturn = ReadString();


            Npc npc;
            if (!(npc = chr.Location.Map.GetNpc(id))) //todo: change to what is visible
                throw new ModerateViolation("Player attempted to select an invalid NPC.");

            switch (interactionType)
            {
                case Action.Initiate:
                    chr.NpcSession.Start(npc, chr);
                    break;

                default:
                    client.Send("This NPC action ({0}) has not yet been implemented.");
                    break;
            }
            return false;
        }
    }
}