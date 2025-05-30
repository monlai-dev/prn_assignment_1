using NewManagementSystem.Models;
using NewManagementSystem.Repository;
using NewManagementSystem.Repository.Abstractions;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.ModelsDTO;

namespace NewManagementSystem.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private const int PageSize = 10;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public PagedViewModel<SystemAccount> GetUsers(int? role, string email, int page = 1)
        {
            var users = _accountRepository.FindAll();
            var count = users.Count();

            if (role.HasValue)
            {
                users = users.Where(u => u.AccountRole == role.Value);
            }
            if (!string.IsNullOrEmpty(email))
            {
                users = users.Where(u => u.AccountEmail.Contains(email));
            }

            users = users.Skip((page - 1) * PageSize).Take(PageSize);

            PagedViewModel<SystemAccount> pagedViewModel = new PagedViewModel<SystemAccount>()
            {
                Items = users.ToList(),
                PageNumber = page,
                TotalPages = (int)Math.Ceiling(count / (double)PageSize)
            };

            return pagedViewModel;
        }

        public async Task<SystemAccount?> FindAccountByEmail(string accountEmail)
        {
            return await _accountRepository.FindAccountByEmail(accountEmail);
        }

        public SystemAccount? GetByEmail(string accountEmail)
        {
            return _accountRepository.GetByEmail(accountEmail);
        }

        public async Task<SystemAccount?> CreateAccount(SystemAccount newAccount)
        {
            return await _accountRepository.CreateAccount(newAccount);
        }
        public async Task<SystemAccount?> FindAccountByUserName(string accountName)
        {
            return await _accountRepository.FindAccountByUserName(accountName);
        }
    }
}