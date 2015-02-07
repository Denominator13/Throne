using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Throne.Framework.Collections;
using Throne.Framework.Runtime;
using Throne.World.Managers;
using Throne.World.Security;
using Throne.World.Structures.Battle.Targeting;
using Throne.World.Structures.Objects;
using Throne.World.Structures.Storage;

namespace Throne.World.Structures.Battle
{
    public partial class Magic : IDisposableResource
    {
        private static readonly ObjectPool<Magic> Pool;

        static Magic()
        {
            Pool = new ObjectPool<Magic>(() =>
            {
                BattleManager.Instance.Log.Info("New mythological power is being summoned from the unknown...");
                return new Magic();
            });
        }

        private Magic()
        {
            Skills = new SkillStorage();
            Targets = new TargetList(this);
        }

        public void Dispose()
        {
            Cleanup();
            Skills.Clear();
            Caster = null;
            IsDisposed = true;

            Pool.Drop(this);
        }

        public Boolean IsDisposed { get; private set; }

        public void Cleanup()
        {
            State = CastState.Finished;
            CurrentSkill = null;
            Targets.Clear();
        }

        public void Execute(BattleInteraction usage)
        {
            Usage = usage;

            try
            {
                Initialize();
                Performable();
                Targeting();
                Perform();
            }
            catch (BattleInteractionException ex)
            {
                Caster.Send(ex.Message);
            }
            catch (Exception ex) //Catch all exceptions to prevent failure to clean up.
            {
                BattleManager.Instance.Log.Exception(ex, "Exception caught for {0} ({1})", Caster, ex.Message);
            }

            Cleanup();
        }

        public static Magic ObtainMagic(Role caster)
        {
            var magic = Pool.Get();
            magic.SetCaster(caster);
            return magic;
        }

        public void AddSkill(params MagicSkill[] skills)
        {
            foreach (MagicSkill skill in skills)
                Skills.Add(skill.Type, skill);
        }

        private void SetCaster(Role caster)
        {
            Caster = caster;
        }

        public static implicit operator Boolean(Magic magic)
        {
            return magic != null;
        }
    }
}