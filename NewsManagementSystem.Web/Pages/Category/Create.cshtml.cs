using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.Services.Services.Abstractions;
using System.Security.Claims;

namespace NewsManagementSystem.Web.Pages.Category;

[Authorize(Roles = "1")]
public class CreateModel : PageModel
{
    private readonly ICategoryServices _service;
    private readonly IAccountService _accountService;
    private readonly IHubContext<DataHub> _hubContext;

    public CreateModel(ICategoryServices service, IAccountService accountService, IHubContext<DataHub> hubContext)
    {
        _service = service;
        _accountService = accountService;
        _hubContext = hubContext;
    }

    [BindProperty]
    public NewManagementSystem.Models.Category Category { get; set; } = new();

    public SelectList ParentCategories { get; set; }

    public async Task<IActionResult> OnGetAsync()
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
        LoadCategories();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await SetUserInfoAsync();

        if (!ModelState.IsValid)
        {
            LoadCategories();
            return Page();
        }

        _service.Add(Category);

        // Broadcast via SignalR after adding
        await _hubContext.Clients.All.SendAsync("CategoryChanged", new
        {
            action = "created",
            id = Category.CategoryId,
            name = Category.CategoryName,
            description = Category.CategoryDesciption,
            parentName = _service.GetById(Category.ParentCategoryId ?? 0)?.CategoryName,
            isActive = Category.IsActive ?? false
        });

        return RedirectToPage("Index");
    }

    private void LoadCategories()
    {
        ParentCategories = new SelectList(_service.GetAll(), "CategoryId", "CategoryName");
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
