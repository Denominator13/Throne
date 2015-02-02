using System;
using Remotion.Linq.Parsing;
using Throne.Framework;
using Throne.World.Database.Client.Files;
using Throne.World.Database.Records;
using Throne.World.Managers;
using Throne.World.Network;
using Throne.World.Network.Messages;
using Throne.World.Records;

namespace Throne.World.Structures.Battle
{
    public class MagicSkill
    {
        public readonly MagicRecord Record;
        private readonly MagicTemplate _template;

        public MagicSkill(MagicRecord record)
        {
            Record = record;
            _template = BattleManager.MagicTemplates[Type];
        }

        public MagicTemplate Template
        {
            get
            {
                if (_template != null)
                    return _template;
                throw new Exception("Template does not exist for skill ID {0}".Interpolate(Type));
            }
        }

        public Int32 Type
        {
            get { return Record.Type; }
        }

        public Byte Level
        {
            get
            {
                return
                    Record.Level;
            }
            set
            {
                Record.Level = value;
                Record.Update();
            }
        }

        public UInt32 Experience
        {
            get { return Record.Experience; }
            set { Record.Experience = value; }
        }

        /// <summary>
        ///     Don't remove/delete skills
        ///     It's pointless and souls can be lost unless removed before rebirth/reincarnation.
        /// </summary>
        public Boolean Available
        {
            get { return Record.Available; }
            set
            {
                Record.Available = value;
                Record.Update();
            }
        }

        public static MagicSkill Create(CharacterRecord chrRecord, Int32 type, Byte level)
        {
            var data = new MagicRecord(chrRecord) {Type = type, Level = level, Available = true};
            data.Create();
            return new MagicSkill(data);
        }

        public static implicit operator WorldPacket(MagicSkill skill)
        {
            return new SkillInformation(skill);
        }
    }
}