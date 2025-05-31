using System.Collections.Generic;
using NewManagementSystem.Models;

namespace NewsManagementSystem.Controllers.ViewModels;

public class PagedUsersViewModel
{
    public IEnumerable<SystemAccount> scam { get; set; }
}