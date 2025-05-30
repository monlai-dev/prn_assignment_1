using NewManagementSystem.Models;
using NewsManagementSystem.BusinessObject.ModelsDTO;

namespace NewManagementSystem.Services.Abstractions
{
    public interface IAccountService
    {
        PagedViewModel<SystemAccount> GetUsers(int? role, string email, int page = 1);

        Task<SystemAccount?> FindAccountByEmail(string accountEmail);

        SystemAccount? GetByEmail(string accountEmail);
        Task<SystemAccount?> CreateAccount(SystemAccount newAccount);

		Task<SystemAccount?> FindAccountByUserName(string accountName);
	}
}