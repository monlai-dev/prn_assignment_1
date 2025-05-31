using Microsoft.EntityFrameworkCore;
using NewManagementSystem.Models;
using NewManagementSystem.Repository.Abstractions;
using NewsManagementSystem.DataAccess;

namespace NewManagementSystem.Repository
{
	public class AccountRepository : Repository<SystemAccount>, IAccountRepository
	{
		public readonly FunewsManagementContext _context;
		public AccountRepository(FunewsManagementContext context) : base(context)
		{
			_context = context;
		}

		public async Task<SystemAccount?> FindByIdAsync(short id, CancellationToken cancellationToken = default)
		{
			return await FindSingleAsync(account => account.AccountId == id, cancellationToken);
		}

		public async Task<SystemAccount?> FindAccountByEmail(string accountEmail)
		{
			return await FindSingleAsync(account => account.AccountEmail == accountEmail);
		}
		public SystemAccount? GetByEmail(string accountEmail)
		{
			return _context.SystemAccounts.FirstOrDefault(account => account.AccountEmail == accountEmail);
		}
		public async Task<SystemAccount?> CreateAccount(SystemAccount newAccount)
		{
			var existingAccount = await FindSingleAsync(account => account.AccountEmail == newAccount.AccountEmail);
			if (existingAccount != null)
			{
				return null;
			}

			using (var transaction = await _context.Database.BeginTransactionAsync())
			{
				int maxId = 0;
				if (await _context.SystemAccounts.AnyAsync())
				{
					maxId = await _context.SystemAccounts.MaxAsync(a => a.AccountId);
				}
				newAccount.AccountId = (short)(maxId + 1);

				await _context.SystemAccounts.AddAsync(newAccount);
				await _context.SaveChangesAsync();

				await transaction.CommitAsync();
			}

			return newAccount;
		}

		public async Task<SystemAccount?> FindAccountByUserName(string accountName)
		{
			return await FindSingleAsync(account => account.AccountName == accountName);
		}
	}
}
