using System;
using Throne.Framework.Network.Connectivity;
using Throne.Framework.Network.Transmission;
using Throne.Framework.Security.Permissions;
using Throne.World.Network.Handling;
using Throne.World.Properties;
using Throne.World.Records;

namespace Throne.World.Network.Messages
{
    [WorldPacketHandler(PacketTypes.AuthenticateClient)]
    public class ClientAuthentication : WorldPacket
    {
        private int _password, _session;

        /// <summary>
        ///     Incoming constructor.
        /// </summary>
        /// <param name="array">Incoming byte array.</param>
        public ClientAuthentication(byte[] array)
            : base(array)
        {
        }

        public override bool Read(WorldClient client)
        {
            _password = ReadInt();
            _session = ReadInt();
            return true;
        }

        public override void Handle(WorldClient client)
        {
            WorldServer.Instance.AccountService.Call(accService =>
            {
                if (!accService.Authorize(_session, _password))
                    return;

                client.AccountData = accService.GetAccount(_session);
                client.AddPermission(new AuthenticatedPermission());
                accService.SetOnline(client.AccountData.UserGuid, true);
            });

            if (client.HasPermission(typeof(AuthenticatedPermission)))
            {
                client.SendArrays(
                    Constants.LoginMessages.ServerInfo,
                    Constants.LoginMessages.AnswerOk,
                    new TimeSynchronize(DateTime.Now));

                CharacterRecord chr = CharacterManager.Instance.FindCharacterRecord(client);

                if (chr == null)
                {
                    client.Send(Constants.LoginMessages.NewRole);
                    return;
                }
                client.SetCharacter(CharacterManager.Instance.InitiaizeCharacter(client, chr));
            }
            else
                client.DisconnectWithMessage(Constants.LoginMessages.BadAuthentication);
        }
    }
}