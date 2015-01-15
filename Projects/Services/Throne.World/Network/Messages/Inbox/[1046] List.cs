using System;
using Throne.Framework.Network.Transmission;
using Throne.World.Managers;
using Throne.World.Network.Handling;
using Throne.World.Structures.Mail;
using Throne.World.Structures.Objects;

namespace Throne.World.Network.Messages.Inbox
{
    [WorldPacketHandler(PacketTypes.MailList)]
    public class List : WorldPacket
    {
        public List(Byte[] array)
            : base(array)
        {
        }

        /// <summary>
        ///     This structure is not proper, but works in the way desired.
        ///     I've split two ints, using them to do my bidding. -Scotty
        /// </summary>
        /// <param name="list"></param>
        /// <param name="page"></param>
        /// <param name="more"></param>
        public List(Int32 page, Boolean more, params Mail[] mails)
            : base(PacketTypes.MailList, 88*mails.Length + 16 + 8)
        {
            //write count for this send
            WriteInt(mails.Length);

            //write paging info
            Seek(9);
            WriteByte((byte) page);
            WriteByte((byte) page);

            //write true if there is more mail left
            Seek(15);
            WriteBoolean(more);

            foreach (Mail mail in mails)
            {
                WriteUInt(mail.Id);
                WriteString(mail.Sender);
                SeekForward(32 - mail.Sender.Length);
                WriteString(mail.Header);
                SeekForward(32 - mail.Header.Length);
                WriteUInt(mail.Money);
                WriteUInt(mail.EMoney);
                WriteInt((int) (DateTime.Now - mail.Creation).TotalSeconds);
                WriteUInt(mail.Item ? mail.Item.ID : 0);
                WriteInt(0);
            }
        }

        public override bool Read(WorldClient client)
        {
            Character c = client.Character;

            bool Refresh = Seek(8).ReadByte() == 0;
            byte RequestPage = ReadByte();
            byte CurrentPage = ReadByte();
            bool GetNextPage = RequestPage == CurrentPage && !Refresh;

            if (GetNextPage)
                RequestPage++;

            MailManager.Instance.SendList(c, RequestPage);
            return false;
        }
    }
}