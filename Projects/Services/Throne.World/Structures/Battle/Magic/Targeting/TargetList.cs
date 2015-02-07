using System.Collections.Generic;
using System.Linq;
using Throne.Framework.Network.Transmission.Stream;
using Throne.World.Network;
using Throne.World.Network.Messages;

namespace Throne.World.Structures.Battle.Targeting
{
    public class TargetList : List<Target>
    {
        private readonly Magic _magic;

        public TargetList(Magic magic)
        {
            _magic = magic;
        }


        public static implicit operator WorldPacket(TargetList tgtList)
        {
            return tgtList.Count < 30 ? new SkillEffect(tgtList, tgtList._magic) : null;
        }

        public static implicit operator Stream(TargetList tgtList)
        {
            var stream = new Stream();
            for (int i = 0; i < tgtList.Count; i++)
                stream.Join(new SkillEffect(tgtList.Skip(i*30).Take(30).ToArray(), tgtList._magic));
            return stream;
        }
    }
}