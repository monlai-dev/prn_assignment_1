using NewManagementSystem.Models;
using NewManagementSystem.Repository.Abstractions;
using NewsManagementSystem.DataAccess;

namespace NewManagementSystem.Repository
{
    public class AccountRepository : Repository<SystemAccount>, IAccountRepository
    {
        public AccountRepository(FunewsManagementContext context) : base(context)
        {
        }

        public async Task<SystemAccount?> FindByIdAsync(short id, CancellationToken cancellationToken = default)
        {
            return await FindSingleAsync(account => account.AccountId == id, cancellationToken);
        }
    }
}
