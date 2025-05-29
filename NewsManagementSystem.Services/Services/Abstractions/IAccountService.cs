using NewManagementSystem.Models;
using NewsManagementSystem.BusinessObject.ModelsDTO;

namespace NewManagementSystem.Services.Abstractions
{
    public interface IAccountService
    {
        PagedViewModel<SystemAccount> GetUsers(int? role, string email, int page = 1);
    }
}