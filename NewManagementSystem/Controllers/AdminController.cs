using Microsoft.AspNetCore.Mvc;
using NewManagementSystem.Services.Abstractions;

namespace NewManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAccountService _accountService;

        public AdminController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: /Admin/Users?role=1&email=abc@xyz.com
        public IActionResult Users(int? role, string? email)
        {
            var users = _accountService.GetUsers(role, email);
            return View(users.ToList());
        }
    }
}
