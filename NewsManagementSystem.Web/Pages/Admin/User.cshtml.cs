using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Common;
using NewsManagementSystem.BusinessObject.Configuration;
using System.Security.Claims;

namespace NewsManagementSystem.Web.Pages.Admin
{

	[Authorize(Roles = "3")]
	public class UserModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IHubContext<DataHub> _hubContext;

        public UserModel(IAccountService accountService, IHubContext<DataHub> hubContext)
        {
            _accountService = accountService;
            _hubContext = hubContext;
        }

        public PagedViewModel<SystemAccount> Users { get; set; } = new();

        [BindProperty]
        public SystemAccount UserInput { get; set; } = new();

        [BindProperty]
        public short DeleteUserId { get; set; }

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

            Users = (_accountService.GetUsers(null, ""));
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            await _accountService.CreateAccount(UserInput);
            await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            _accountService.Update(UserInput.AccountId, UserInput.AccountName, UserInput.AccountEmail, UserInput.AccountRole, UserInput.AccountPassword);
            await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            // Implement your delete logic here, e.g. _accountService.Delete(UserInput.AccountId);
            await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate");
            return RedirectToPage();
        }
    }
}
