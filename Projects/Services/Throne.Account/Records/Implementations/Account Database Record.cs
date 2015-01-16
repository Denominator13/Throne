using Throne.Framework.Persistence.Interfaces;

namespace Throne.Login.Records.Implementations
{
    public abstract class AccountDatabaseRecord : IActiveRecord
    {
        public virtual void Create()
        {
            LoginServer.Instance.AccountDbContext.PostAsync(x => x.Commit(this));
        }

        public virtual void Update()
        {
            LoginServer.Instance.AccountDbContext.PostAsync(x => x.Update(this));
        }

        public virtual void Delete()
        {
            LoginServer.Instance.AccountDbContext.PostAsync(x => x.Delete(this));
        }
    }
}