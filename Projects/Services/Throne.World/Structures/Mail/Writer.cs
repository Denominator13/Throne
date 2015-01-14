using System;
using System.Text;
using Throne.Framework;
using Throne.World.Database.Records;
using Throne.World.Managers;
using Throne.World.Records;

namespace Throne.World.Structures.Mail
{
    public sealed class MailWriter
    {
        private readonly StringBuilder _contentBuilder;
        private readonly MailRecord _record;

        public MailWriter(CharacterRecord recipient)
        {
            _record = new MailRecord(MailManager.Instance.SerialGenerator.Next());
            _record.Recipient = recipient;
            _record.Creation = DateTime.Now;

            _contentBuilder = new StringBuilder();
        }

        public MailWriter SetSender(String name)
        {
            _record.Sender = name.Truncate(32);
            return this;
        }

        public MailWriter SetHeader(String text)
        {
            _record.Header = text.Truncate(32);
            return this;
        }

        public MailWriter WriteLine(String text, Boolean dbl = false)
        {
            _contentBuilder.Append(text).Append(dbl ? "\n\n" : "\n");
            return this;
        }

        public MailWriter WriteMargin()
        {
            _contentBuilder.Append("________________________________\n\n");
            return this;
        }

        public MailWriter AttachMoney(UInt32 amount)
        {
            _record.Money = amount;
            return this;
        }

        public MailWriter AttachEMoney(UInt32 amount)
        {
            _record.EMoney = amount;
            return this;
        }

        public MailWriter AttachItem(ItemRecord item)
        {
            _record.Item = item;
            return this;
        }

        public void Send()
        {
            _record.Content = _contentBuilder.ToString().Truncate(255);
            _record.Create();
        }
    }
}