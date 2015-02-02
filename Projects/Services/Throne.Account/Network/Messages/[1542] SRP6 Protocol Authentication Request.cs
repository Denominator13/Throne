using System;
using Throne.Framework.Network.Transmission;
using Throne.Login.Accounts;
using Throne.Login.Network.Handling;
using Throne.Login.Records;

namespace Throne.Login.Network.Messages
{
    [AuthenticationPacketHandler(PacketTypes.Srp6ProtocolAuthenticationRequest)]
    public sealed class SRP6ProtocolAuthenticationRequest : LoginPacket
    {
        private const int LENGTH = 312;

        private String MacAddress;
        private String Password;
        private String Server;
        private String Username;

        /// <summary>
        ///     Incoming constructor.
        /// </summary>
        /// <param name="array">Incoming byte array.</param>
        public SRP6ProtocolAuthenticationRequest(byte[] array)
            : base(array)
        {
        }

        public override bool Read(LoginClient client)
        {
            if (ArrayLength != LENGTH)
                InvalidValue(client, "Length", ArrayLength, LENGTH);

            Username = Seek(8).ReadString(64);
            Password = ReadString(64);
            Server = ReadString(16);
            MacAddress = ReadString(12);

            //Unknown byte = SeekForward(30)
            //SRP6 info = SeekForward(49), 64 bytes
            return true;
        }

        public override void Handle(LoginClient client)
        {
            //TODO: Transfer to the game server based on the server name... (must communicate server names on connection with the account service)

            Account userRecord;
            if (AccountManager.Instance.FindAccount(x => x.Username == Username, out userRecord))
            {
                if (!userRecord.Password.Equals(Password))
                {
                    using (var packet = new AuthenticationAction((int) AuthenticationAction.Type.InvalidCredentials))
                        client.Send(packet);
                    return;
                }


                if (!userRecord.Online)
                {
                    userRecord.MacAddress = MacAddress;
                    userRecord.LastLogin = DateTime.Now;
                    userRecord.LastIP = client.ClientAddress;

                    using (
                        var packet = new AuthenticationAction(userRecord.Guid, userRecord.Password.GetHashCode(),
                            LoginServer.Configuration.Network.GamePort, LoginServer.Configuration.Network.GameIP))
                        client.Send(packet);
                }
                else
                    using (var packet = new AuthenticationAction((int) AuthenticationAction.Type.TryAgainLater))
                        client.Send(packet);

                return;
            }

            LoginServer.Instance.AccountDbContext.Commit(new AccountRecord(Username, Password, ""));
            AccountManager.Instance.LoadAccounts();
            if (AccountManager.Instance.FindAccount(acc => acc.Username == Username, out userRecord))
            {
                userRecord.MacAddress = MacAddress;
                userRecord.LastLogin = DateTime.Now;
                userRecord.LastIP = client.ClientAddress;

                using (
                    var packet = new AuthenticationAction(userRecord.Guid, userRecord.Password.GetHashCode(),
                        LoginServer.Configuration.Network.GamePort, LoginServer.Configuration.Network.GameIP))
                    client.Send(packet);
            }
        }
    }
}