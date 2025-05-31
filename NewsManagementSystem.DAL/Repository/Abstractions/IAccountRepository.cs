using NewManagementSystem.Models;
using NewManagementSystem.Repository.Abstractions;

namespace NewsManagementSystem.DataAccess.Repository.Abstractions
{
    public interface IAccountRepository: IRepository<SystemAccount>
    {
        Task<SystemAccount?> FindAccountByEmail(string accountEmail);
        SystemAccount GetByEmail(string accountEmail);
        Task<SystemAccount?> CreateAccount(SystemAccount newAccount);

		    Task<SystemAccount?> FindAccountByUserName(string accountName);
        Task<SystemAccount?> FindByIdAsync(short id, CancellationToken cancellationToken = default);
    }  
}
