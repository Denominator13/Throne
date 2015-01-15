using System;
using System.Reflection;

namespace Throne.Framework.Network.Handling
{
    public abstract class PacketHandlerBase
    {
        public PacketHandlerBase(ConstructorInfo constructor, Enum typeId, Type permission)
        {
            TypeId = typeId;
            Permission = permission;
            Constructor = constructor;
        }

        public ConstructorInfo Constructor { get; private set; }

        public Type Permission { get; private set; }

        public Enum TypeId { get; private set; }
    }
}