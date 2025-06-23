using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.Services.Services.Abstractions;
using System.Security.Claims;

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
		if (User.Identity?.IsAuthenticated ?? false)
		{
			ViewData["UserInfo"] = new LoginDTO
			{
				AccountName = User.Identity.Name,
				AccountEmail = User.Identities.FirstOrDefault()?.Claims
					.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
			};
		}
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
