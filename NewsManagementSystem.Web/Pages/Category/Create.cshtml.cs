using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsManagementSystem.Services.Services.Abstractions;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

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
            return Partial("_CreateCategoryPartial", this); // Optional: if using partials for validation
        }

        _service.Add(Category);
        await _service.SaveAsync(); // <-- This is REQUIRED to actually save to database

        var parentCategory = _service.GetById(Category.ParentCategoryId ?? 0);

        await _hubContext.Clients.All.SendAsync("CategoryChanged", new
        {
            action = "created",
            id = Category.CategoryId,
            name = Category.CategoryName,
            description = Category.CategoryDesciption,
            parentName = parentCategory?.CategoryName,
            isActive = Category.IsActive ?? false
        });

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return new JsonResult(new { success = true });
        }

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
