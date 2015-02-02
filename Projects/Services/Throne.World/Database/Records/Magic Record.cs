using System;
using Throne.Framework.Persistence.Mapping;
using Throne.World.Database.Records.Implementations;
using Throne.World.Network.Messages;
using Throne.World.Records;

namespace Throne.World.Database.Records
{
    public class MagicRecord : WorldDatabaseRecord
    {
        protected MagicRecord() { }

        public MagicRecord(CharacterRecord record)
        {
            Owner = record;
        }

        public virtual Int32 Id { get; set; }
        public virtual CharacterRecord Owner { get; set; }
        public virtual Int32 Type { get; set; }
        public virtual Boolean Available { get; set; }
        public virtual Byte Level { get; set; }
        public virtual UInt32 Experience { get; set; }
        public virtual Int32 CurrentSoul { get; set; }
        public virtual SkillInformation.SkillSoul Souls { get; set; }
    }

    public class MagicMapping : MappableObject<MagicRecord>
    {
        public MagicMapping()
        {
            Id(r => r.Id);

            References(r => r.Owner);

            Map(r => r.Type);
            Map(r => r.Available);
            Map(r => r.Level);
            Map(r => r.Experience);
            Map(r => r.CurrentSoul);
            Map(r => r.Souls);
        }
    }
}