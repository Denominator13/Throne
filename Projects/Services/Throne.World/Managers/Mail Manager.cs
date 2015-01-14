using System;
using System.Collections.Generic;
using System.Linq;
using Throne.Framework;
using Throne.Framework.Logging;
using Throne.Framework.Threading;
using Throne.World.Database.Records;
using Throne.World.Database.Records.Implementations;
using Throne.World.Network.Messages;
using Throne.World.Network.Messages.Inbox;
using Throne.World.Security;
using Throne.World.Structures.Mail;
using Throne.World.Structures.Objects;

namespace Throne.World.Managers
{
    public sealed class MailManager : SingletonActor<MailManager>
    {
        public readonly LogProxy Log;
        public readonly SerialGenerator SerialGenerator;

        private MailManager()
        {
            Log = new LogProxy("MailManager");

            SerialGeneratorManager.Instance.GetGenerator(typeof (MailRecord).Name, WorldObject.ItemIdMin,
                WorldObject.ItemIdMax, ref SerialGenerator);
        }

        public void CheckUnread(Character forChr)
        {
            if (!forChr.Inbox.HasMail) return;
            if (!forChr.Inbox.UnreadMail) return;
            NotifyOfNewMail(forChr);
        }

        public void NotifyOfNewMail(Character chr, String from = "")
        {
            using (var pkt = new Notify(Notify.Types.UnreadMail))
                chr.User.Send(pkt);

            chr.User.Send(!string.IsNullOrEmpty(@from)
                ? "New mail from {0}".Interpolate(@from)
                : "You've got unread mail.");
        }

        public void SendList(Character forChr, Int32 set)
        {
            Inbox inbox = forChr.Inbox;

            if (inbox.HasMail)
                PostAsync(delegate
                {
                    IEnumerable<Mail> enumerable = inbox.GetMails(set);
                    Mail[] mail = enumerable.Skip(7*set).Take(7).ToArray();
                    bool more = inbox.Count > 7*(set + 1);

                    using (var pkt = new List(set, more, mail))
                        forChr.User.Send(pkt);
                });
            else
                using (var pkt = new List(set, false))
                    forChr.User.Send(pkt);
        }

        public void Open(Character forChr, UInt32 id)
        {
            PostAsync(delegate
            {
                Mail mail;
                if (forChr.Inbox.TryGetValue(id, out mail))
                    using (var mailPkt = new Content(mail.Content, id))
                        if (mail.Item)
                            using (var itemPkt = new ItemInformation(mail.Item, ItemInformation.Mode.Mail))
                                forChr.User.SendArrays(mailPkt, itemPkt);
                        else forChr.User.Send(mailPkt);
                else
                    throw new ModerateViolation("Invalid mail ID.");
            });
        }

        public Boolean Delete(Character forChr, UInt32 id)
        {
            Mail mail;
            if (!forChr.Inbox.TryGetValue(id, out mail))
                throw new ModerateViolation("Invalid mail ID.");

            if (!mail.ContainsAttachment)
            {
                PostAsync(mail.Delete);
                forChr.Inbox.Remove(id);
                return true;
            }

            using (var pkt = new Notify(Notify.Types.DeletionFailed))
                forChr.User.Send(pkt);
            return false;
        }

        public Boolean RemoveItemAttachment(Character forChr, UInt32 id)
        {
            Mail mail;
            if (!forChr.Inbox.TryGetValue(id, out mail))
                throw new ModerateViolation("Invalid mail ID.");

            if (!mail.Item)
                throw new ModerateViolation("Invalid mail item.");

            Item item = mail.Item;
            mail.Item = null;
            forChr.AddItem(item);

            return true;
        }

        public Boolean RemoveMoneyAttachment(Character forChr, UInt32 id)
        {
            Mail mail;
            if (!forChr.Inbox.TryGetValue(id, out mail))
                throw new ModerateViolation("Invalid mail ID.");

            if (mail.Money > 0)
                forChr.Money += mail.Money;
            mail.Money = 0;

            return true;
        }

        public Boolean RemoveEMoneyAttachment(Character forChr, UInt32 id)
        {
            Mail mail;
            if (!forChr.Inbox.TryGetValue(id, out mail))
                throw new ModerateViolation("Invalid mail ID.");

            if (mail.EMoney > 0)
                forChr.EMoney += mail.EMoney;
            mail.EMoney = 0;

            return true;
        }
    }
}