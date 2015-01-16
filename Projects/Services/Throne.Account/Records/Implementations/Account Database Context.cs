using System.Collections.Generic;
using FluentNHibernate;
using Throne.Framework.Persistence;

namespace Throne.Login.Records.Implementations
{
    public sealed class AccountDatabaseContext : GameDatabaseContext
    {
        protected override IEnumerable<IMappingProvider> CreateMappings()
        {
            yield return new AccountMapping();
        }
    }
}