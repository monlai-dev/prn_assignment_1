using Microsoft.AspNetCore.Mvc;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.Controllers.ViewModels;
using System.Linq;

namespace NewsManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAccountService _accountService;

        public AdminController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: /Admin/Users?role=1&email=abc@xyz.com&page=1
        public IActionResult Users(int? role, string? email, int page = 1)
        {
            var pagedResult = _accountService.GetUsers(role, email, page);

            return View(pagedResult);
        }
    }
}
