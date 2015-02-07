using System;
using System.Collections.Generic;
using Throne.Framework;
using Throne.Framework.Logging;
using Throne.Framework.Threading;
using Throne.World.Database.Client;
using Throne.World.Database.Client.Files;
using Throne.World.Structures.Battle.Targeting;
using Throne.World.Structures.Battle.Targeting.Filters;

namespace Throne.World.Managers
{
    public sealed class BattleManager : SingletonActor<BattleManager>
    {
        public static Dictionary<Int32, MagicTemplate> MagicTemplates;
        public Logger Log;

        private BattleManager()
        {
            Log = new Logger("BattleManager");
        }

        public void Load()
        {
            ClientDatabaseReader.Read("magictype.dat", out MagicTemplates);
            // over 65 magic logic types.... phew here we go
            AssignMagicTargetCollectors();
            Log.Info("Ready.");
        }

        private static void AssignMagicTargetCollectors()
        {
            MagicTemplates.Values.With(template =>
            {
                switch (template.Sort)
                {
                    case MagicSort.SingleTarget:
                        template.TargetCollector = new TargetCollection(CollectorType.Explicit, new AttackableFilter());
                        break;
                }
            });
        }
    }
}