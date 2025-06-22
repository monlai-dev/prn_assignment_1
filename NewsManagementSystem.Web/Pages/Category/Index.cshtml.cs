using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using NewsManagementSystem.Services.Services.Abstractions;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;

namespace NewsManagementSystem.Web.Pages.Category;

[Authorize(Roles = "1")]
public class CategoryIndexModel : PageModel
{
    private readonly ICategoryServices _service;
    private readonly IAccountService _accountService;

    public CategoryIndexModel(ICategoryServices service, IAccountService accountService)
    {
        _service = service;
        _accountService = accountService;
    }

    public IEnumerable<NewManagementSystem.Models.Category> Categories { get; set; }

    public async Task OnGetAsync()
    {
        await SetUserInfoAsync();
        Categories = _service.GetAll();
    }

    private async Task SetUserInfoAsync()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            var user = await _accountService.FindAccountByUserName(User.Identity.Name);
            if (user != null)
            {
                ViewData["UserInfo"] = new
                {
                    user.AccountName,
                    user.AccountEmail
                };
            }
        }
    }
}
