using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.DataAccess.Repository.Abstractions;

namespace NewsManagementSystem.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public IEnumerable<SystemAccount> GetUsers(int? role, string email)
        {
            var users = _accountRepository.FindAll();

            if (role.HasValue)
            {
                users = users.Where(u => u.AccountRole == role.Value);
            }
            if (!string.IsNullOrEmpty(email))
            {
                users = users.Where(u => u.AccountEmail.Contains(email));
            }

            return users;
        }
    }
}