using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using Throne.Framework.Logging;
using Throne.Framework.Network.Connectivity;
using Throne.Framework.Network.Transmission;
using Throne.Framework.Reflection;
using Throne.Framework.Security.Permissions;

namespace Throne.Framework.Network.Handling
{
    public abstract class PacketPropagatorBase<TAttribute, TPacket, THandler> : IPacketPropagator
        where TAttribute : PacketHandlerAttribute
        where TPacket : Packet
        where THandler : PacketHandlerBase
    {
        private static readonly Logger Log = new Logger("PacketPropagatorBase");

        private readonly ConcurrentDictionary<Int16, THandler> _handlers =
            new ConcurrentDictionary<Int16, THandler>();

        protected PacketPropagatorBase()
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                CacheHandlers(asm);
            Log.Info("{0} Packet Handler classes cached.", _handlers.Count);
        }

        public void Handle<TClient>(TClient client, short typeId, byte[] payload, short length) where TClient : IClient
        {
            dynamic handler = GetHandler(typeId);

            Log.Debug(
                "{0}\n{1}/{2}:{3}\t{4}\n{5}",
                client,
                typeId.ToString("X2", CultureInfo.InvariantCulture),
                typeId,
                ((PacketTypes) typeId),
                length,
                BitConverter.ToString(payload).Replace("-", " ").WordWrap(48)
                );

            if (handler == null) return;

            Type permission = handler.Permission;
            if (!client.HasPermission(permission))
            {
                Log.Warn("Client {0} sent type {1} which requires permission {2} - disconnected.",
                    client.ClientAddress,
                    typeId.ToString("X2", CultureInfo.InvariantCulture), permission.Name);
                client.Disconnect();
                return;
            }

            //leave as lambada, gives a complete stack trace.
            client.PostAsync(() => handler.Invoke(client, payload));
        }

        private void CacheHandlers(Assembly asm)
        {
            foreach (Type type in asm.GetTypes())
            {
                var attr = ReflectionExtensions.GetCustomAttribute<TAttribute>(type);
                if (attr == null) continue;

                Type parent = typeof (TPacket);
                if (!type.IsAssignableTo(parent))
                {
                    Log.Error("{0} classes must inherit from {1}", attr, parent);
                    continue;
                }

                ConstructorInfo ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null,
                    new[] {typeof (Byte[])}, null);
                if (ctor == null)
                {
                    Log.Error("{0} needs a public instance constructor with one type argument as a byte array.",
                        type.Name);
                    continue;
                }

                Enum typeId = attr.PacketTypeId;
                var handler = (THandler)Activator.CreateInstance(typeof(THandler), ctor, typeId, attr.Permission ?? typeof(ConnectedPermission));

                AddHandler(((IConvertible) typeId).ToInt16(null), handler);
            }
        }

        private void AddHandler(Int16 typeId, THandler handler)
        {
            _handlers.Add(typeId, handler);
        }

        private THandler GetHandler(Int16 typeId)
        {
            return _handlers.TryGet(typeId);
        }
    }
}