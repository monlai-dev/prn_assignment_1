using NewManagementSystem.Models;

namespace NewManagementSystem.Services.Abstractions
{
    public interface IAccountService
    {
        IEnumerable<SystemAccount> GetUsers(int? role, string email);
    }
}