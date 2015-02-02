﻿using System;
using Throne.Framework.Collections;
using Throne.World.Network;
using Throne.World.Records;
using Throne.World.Sessions;
using Throne.World.Structures.Mail;

namespace Throne.World.Structures.Objects
{
    /// <summary>
    ///     This partial class contains all properties of the character.
    /// </summary>
    partial class Character
    {
        /// <summary>
        ///     The database layer accessor for the class.
        /// </summary>
        public CharacterRecord Record { get; private set; }

        /// <summary>
        ///     The user controlling this character.
        /// </summary>
        public WorldClient User { get; private set; }

        /// <summary>
        ///     The current NPC activity session for this character.
        /// </summary>
        public NpcSession NpcSession { get; private set; }

        /// <summary>
        ///     Mail service for this character.
        /// </summary>
        public Inbox Inbox { get; private set; }

        #region Constant Region

        public const Int32
            MAX_MONEY = 1000000000,
            MAX_LUCKYTIME = 7200000;

        #endregion

        #region Group

        #endregion

        #region Battlefields

        #endregion

        #region Financial

        #endregion

        #region Battle

        public Byte Level
        {
            get { return Record.Level; }
            set
            {
                Record.Level = value;
                Record.Update();
            }
        }

        public Profession CurrentJob
        {
            get { return Record.CurrentJob; }
            set
            {
                PreviousJob = Record.CurrentJob;
                Record.CurrentJob = value;
            }
        }

        public Profession PreviousJob
        {
            get { return Record.PreviousJob; }
            set
            {
                AncestorJob = Record.PreviousJob;
                Record.PreviousJob = value;
            }
        }

        public Profession AncestorJob
        {
            get { return Record.AncestorJob; }
            set
            {
                Record.AncestorJob = value;
                Record.Update();
            }
        }

        public UInt16 Strength
        {
            get { return Record.Strength; }
            set { Record.Strength = value; }
        }

        public UInt16 Agility
        {
            get { return Record.Agility; }
            set { Record.Agility = value; }
        }

        public UInt16 Spirit
        {
            get { return Record.Spirit; }
            set { Record.Spirit = value; }
        }

        public UInt16 Vitality
        {
            get { return Record.Vitality; }
            set { Record.Vitality = value; }
        }

        public UInt16 AttributePoints
        {
            get { return Record.AttributePoints; }
            set { Record.AttributePoints = value; }
        }

        public Boolean AlternateGearActive { get; set; }

        #endregion

        #region Social

        private Boolean _away;

        public override String Name
        {
            get { return Record.Name; }
            set
            {
                Record.Name = value;
                Record.Update();
            }
        }

        public Character Spouse
        {
            get
            {
                CharacterRecord spouse = Record.Spouse;
                return spouse != null ? CharacterManager.Instance.FindCharacter(spouse.Guid) : null;
            }
            set
            {
                Record.Spouse = value != null ? value.Record : null;
                Record.Update();
            }
        }

        public override Model Look
        {
            get { return _look; }
            set
            {
                _look = value;
                Record.Look = _look;
                Record.Update();
            }
        }

        public RoleAppearance Appearance
        {
            get { return Record.Appearance; }
            set
            {
                Record.Appearance = value;
                SendToLocal();
            }
        }

        public Boolean Away
        {
            get { return _away; }
            set
            {
                _away = value;
                SendToLocal(this);
            }
        }

        #endregion

        #region Misc

        public Boolean JustCreated { get; internal set; }

        public Boolean LoggedIn { get; set; }

        #endregion

        #region Public Statuses

        private readonly BooleanArray<RoleState> _pStats;

        public Byte[] PublicStates
        {
            get
            {
                lock (_pStats.SyncRoot)
                    return _pStats;
            }
        }

        public void SetPublicState(RoleState type, Boolean value)
        {
            lock (_pStats.SyncRoot)
                _pStats.Set(type, value);
        }

        #endregion
    }
}