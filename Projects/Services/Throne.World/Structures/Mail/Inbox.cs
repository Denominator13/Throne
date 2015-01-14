using System;
using System.Collections.Generic;
using System.Linq;

namespace Throne.World.Structures.Mail
{
    public class Inbox : Dictionary<UInt32, Mail>
    {
        public Inbox(IEnumerable<Mail> payload)
        {
            foreach (Mail mail in payload)
                Add(mail.Id, mail);
        }

        public Boolean NeedsRefresh { get; set; }

        public Boolean UnreadMail
        {
            get { return Values.Any(mail => !mail.Opened || mail.ContainsAttachment); }
        }

        public Boolean HasMail
        {
            get { return Count > 0; }
        }

        public IEnumerable<Mail> GetMails(Int32 set)
        {
            return Values.OrderByDescending(m => m.Creation);
        }
    }
}