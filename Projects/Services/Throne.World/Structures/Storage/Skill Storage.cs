using System;
using System.Collections.Generic;
using Throne.Framework.Network.Transmission.Stream;
using Throne.World.Network;
using Throne.World.Structures.Battle;

namespace Throne.World.Structures.Storage
{
    public class SkillStorage : Dictionary<Int32, MagicSkill>
    {
        public Stream Stream
        {
            get { return this; }
        }

        public static implicit operator Stream(SkillStorage storage)
        {
            var stream = new Stream();
            foreach (MagicSkill skill in storage.Values)
                stream.Join((WorldPacket) skill);
            return stream;
        }
    }
}