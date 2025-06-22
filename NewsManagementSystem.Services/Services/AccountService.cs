using NewManagementSystem.Models;
using NewManagementSystem.Repository;
using NewManagementSystem.Repository.Abstractions;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Common;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.DataAccess;
using NewsManagementSystem.DataAccess.Repository.Abstractions;
using System;
using System.Security.Cryptography;
using System.Text;

namespace NewManagementSystem.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly FunewsManagementContext _context;
        private readonly AdminCredentialsModel _adminCredentials;
        private const int PageSize = 10;

        public AccountService(IAccountRepository accountRepository, FunewsManagementContext context, AdminCredentialsModel adminCredentials)
        {
            _accountRepository = accountRepository;
            _context = context;
            _adminCredentials = adminCredentials;
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

        public async Task<SystemAccount?> GetUserById(int id)
        {
            return await _accountRepository.FindByIdAsync((short)id);
        }

        public bool Update(short accountId, string? accountName, string? accountEmail, int? accountRole, string? accountPassword)
        {
            var user = _accountRepository.FindByIdAsync(accountId).Result;
            if (user == null)
            {
                return false;
            }
            user.AccountName = accountName ?? user.AccountName;
            user.AccountEmail = accountEmail ?? user.AccountEmail;
            user.AccountRole = accountRole ?? user.AccountRole;
            user.AccountPassword = accountPassword; // currently not support for changing password

            _accountRepository.Update(user);
            return _context.SaveChanges() > 0;
        }

        public bool IsAdminLogin(string email, string password)
        {
            return email.Equals(_adminCredentials.Email, StringComparison.OrdinalIgnoreCase) &&
                   password.Equals(_adminCredentials.Password);

        }

        public async Task<(bool success, string message)> UpdateProfileAsync(string currentUsername, string newUsername, string newEmail, string? currentPassword, string? newPassword)
        {
            try
            {
                // Get current user
                var currentUser = await _accountRepository.FindAccountByUserName(currentUsername);
                if (currentUser == null)
                {
                    return (false, "User not found.");
                }

                // Validate current password if trying to change password
                if (!string.IsNullOrEmpty(newPassword))
                {
                    if (string.IsNullOrEmpty(currentPassword))
                    {
                        return (false, "Current password is required to change password.");
                    }

                    if (currentUser.AccountPassword != HashPassword(currentPassword))
                    {
                        return (false, "Current password is incorrect.");
                    }

                    if (newPassword.Length < 6)
                    {
                        return (false, "Password must be at least 6 characters long.");
                    }
                }

                // Check if new username is already taken (if changed)
                if (newUsername != currentUsername)
                {
                    var existingUser = await _accountRepository.FindAccountByUserName(newUsername);
                    if (existingUser != null)
                    {
                        return (false, "Username is already taken.");
                    }
                }

                // Check if new email is already taken (if changed)
                if (newEmail != currentUser.AccountEmail)
                {
                    var existingUser = _accountRepository.GetByEmail(newEmail);
                    if (existingUser != null)
                    {
                        return (false, "Email is already registered.");
                    }
                }

                // Update user information
                currentUser.AccountName = newUsername;
                currentUser.AccountEmail = newEmail;

                // Update password if provided
                if (!string.IsNullOrEmpty(newPassword))
                {
                    currentUser.AccountPassword = HashPassword(newPassword);
                }

                _accountRepository.Update(currentUser);
                var result = _context.SaveChanges();

                if (result > 0)
                {
                    return (true, "Profile updated successfully!");
                }
                else
                {
                    return (false, "Failed to update profile. Please try again.");
                }
            }
            catch (Exception ex)
            {
                return (false, "An error occurred while updating your profile. Please try again.");
            }
        }
        
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(hash);
        }
    }
    
    
}