using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Throne.Framework;
using Throne.Framework.Collections;
using Throne.Framework.Logging;
using Throne.Framework.Runtime;
using Throne.World.Network;
using Throne.World.Network.Messages;
using Throne.World.Properties;
using Throne.World.Records;
using Throne.World.Sessions;
using Throne.World.Structures.Battle;
using Throne.World.Structures.Mail;
using Throne.World.Structures.Objects.Actors;
using Throne.World.Structures.Storage;
using Throne.World.Structures.Travel;

namespace Throne.World.Structures.Objects
{
    /// <summary> An in-game user controlled entity with an archetype. </summary>
    public sealed partial class Character : Role, IDisposableResource
    {
        public static Type Type = typeof(Character);
        public readonly SemaphoreSlim InitializationSignal;

        /// <summary>
        ///     Initializes a new character object.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="record"></param>
        public Character(WorldClient user, CharacterRecord record)
            : base(record.Guid)
        {
            NpcSession = new NpcSession();

            User = user;
            Record = record;

            User.Log = new Logger("{0}:{1}".Interpolate(Name, user.ClientAddress));
            Log.Info(StrRes.SMSG_LoggedIn);

            _timers = new Dictionary<CharacterTask, CharacterTimer>();

            _look = new Model(Record.Look);
            _pStats = new BooleanArray<RoleState>(192);

            List<Item> items = Record.ItemPayload.Select(itemRecord => new Item(itemRecord)).ToList();
            {
                ConstructItemDepositories(ref items);

                _gear = new Gear(ref items);
                _inventory = new Inventory(ref items);
            }

            Inbox = new Inbox(Record.MailPayload.Select(mailRecord => new Mail.Mail(mailRecord)));

            Magic.AddSkill(Record.SkillPayload.Select(skillRecord => new MagicSkill(skillRecord)).ToArray());

            _currentVisibleCharacters = new Dictionary<UInt32, Character>();
            _currentVisibleMapItems = new Dictionary<UInt32, Item>();
            _currentVisibleNpcs = new Dictionary<UInt32, Npc>();

            InitializationSignal = new SemaphoreSlim(0, 1);
            Initialize();
        }

        public Logger Log
        {
            get { return User.Log; }
        }

        public void Dispose()
        {
            if (Location)
                ExitCurrentRegion();

            ClearScreen();
            NpcSession.Clear();
            Magic = null;

            Log.Info(StrRes.SMSG_LoggedOut);
        }

        public bool IsDisposed { get; private set; }

        #region Sending

        public void SendToLocal(WorldPacket packet = null, Boolean includeSelf = false)
        {
            if (!packet) packet = this;
            if (includeSelf) User.Send(packet);

            foreach (Character user in _currentVisibleCharacters.Values)
                user.User.Send(packet);
        }

        public override void SpawnFor(WorldClient observer)
        {
            ExchangeSpawns(observer.Character);
        }

        public override void DespawnFor(WorldClient observer)
        {
            using (var pkt = new GeneralAction(ActionType.RemoveEntity, this))
                observer.Send(pkt);
        }

        public void Send(WorldPacket packet)
        {
            User.Send(packet);
        }

        public override void Send(String msg)
        {
            User.Send(msg);
        }

        public override void ExchangeSpawns(Character with)
        {
            //TODO: goodwill suits

            with.User.Send((RoleInfo) this);
            User.Send((RoleInfo) with);
        }


        public static implicit operator RoleInfo(Character characterToSpawn)
        {
            return new RoleInfo(characterToSpawn);
        }

        #endregion

        public async void Initialize()
        {
            User.PostAsync(() => Send(new LoadMap(Record.MapID)));

            try
            {
                await InitializationSignal.WaitAsync(5000);
            }
            catch (OperationCanceledException)
            {
                Log.Error("Map load signal not received from client.");
                Logout();
                return;
            }

            User.SendArrays(
                new CharacterInformation(this),
                new VIPValidFunctions()
                //mentor
                //goodwill ranks
                //guild
                //title
                );

            EnterRegion(new Location(Record.MapID, Record.X, Record.Y));
        }

        public void Save()
        {
            if (Location)
            {
                Record.X = Location.Position.X;
                Record.Y = Location.Position.Y;
                Record.MapID = Location.MapId;
                Record.InstanceId = Location.Map.Instance;
            }

            Record.Update();
        }

        public void Logout()
        {
            User.Disconnect();
        }

        public override string ToString()
        {
            return Name;
        }

        public void Teleport(Location to)
        {
            if (!to.Map)
                throw new ArgumentException("The map does not exist.");

            using (GeneralAction pkt = new GeneralAction(ActionType.Teleport, this).Teleport(to))
                User.Send(pkt);

            ExitCurrentRegion();
            EnterRegion(to);
        }
    }
}