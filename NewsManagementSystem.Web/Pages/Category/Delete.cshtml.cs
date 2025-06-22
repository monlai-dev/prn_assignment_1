using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using NewsManagementSystem.Services.Services.Abstractions;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace NewsManagementSystem.Web.Pages.Category;

[Authorize(Roles = "1")]
public class DeleteModel : PageModel
{
    private readonly ICategoryServices _service;
    private readonly IAccountService _accountService;
    private readonly IHubContext<DataHub> _hubContext;

    public DeleteModel(ICategoryServices service, IAccountService accountService, IHubContext<DataHub> hubContext)
    {
        _service = service;
        _accountService = accountService;
        _hubContext = hubContext;
    }

    [BindProperty]
    public NewManagementSystem.Models.Category Category { get; set; }

    public async Task<IActionResult> OnGetAsync(short id)
    {
        await SetUserInfoAsync();
        short categoryId = (short)id;
        Category = _service.GetById(id);
        if (Category == null) return NotFound();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(short id)
    {
        await SetUserInfoAsync();
        var success = _service.Delete(id);
        if (!success)
        {
            TempData["Error"] = "Cannot delete this category. It is being used in news articles.";
        }
        else
        {
            // Broadcast via SignalR after deleting
            await _hubContext.Clients.All.SendAsync("CategoryChanged", new
            {
                action = "deleted",
                id = id
            });
        }
        return RedirectToPage("Index");
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
