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
            Effects = new BooleanArray<ResponseEffect>(4);
            Skills = new SkillStorage();
            Targeting = new TargetingCollection();
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
            MagicSkill = null;
            Effects.SetAll(false);
            Targeting.Clear();
        }

        public void Execute(BattleInteraction usage)
        {
            Usage = usage;
            if (!CanPerform()) return;
            if (!PrePerform()) return;

            Perform();

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