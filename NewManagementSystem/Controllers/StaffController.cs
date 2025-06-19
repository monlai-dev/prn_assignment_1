using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.Controllers.ViewModels;

namespace YourProject.Controllers
{
    //[Authorize(Roles = "Staff")]
    public class StaffController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<StaffController> _logger;

        public StaffController(IAccountService accountService, ILogger<StaffController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        // GET: /Staff/EditProfile
        [HttpGet]
        [Route("Staff/EditProfile")]
        public async Task<IActionResult> EditProfile()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");

            var user = await _accountService.FindAccountByUserName(User.Identity.Name);
            if (user == null)
                return NotFound();

            var viewModel = new UserEditViewModel
            {
                AccountId = user.AccountId,
                AccountName = user.AccountName,
                AccountEmail = user.AccountEmail,
                AccountRole = user.AccountRole 
            };

            ViewBag.UserInfo = new LoginDTO
            {
                AccountName = user.AccountName,
                AccountEmail = user.AccountEmail
            };

            return View("EditUser", viewModel);
        }
        // POST: /Staff/EditProfile
        [HttpPost]
        [Route("Staff/EditProfile")]
        public async Task<IActionResult> EditProfile(UserEditViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");

            var user = await _accountService.FindAccountByUserName(User.Identity.Name);
            if (user == null)
                return NotFound();

            model.AccountId = user.AccountId;

            ViewBag.UserInfo = new LoginDTO
            {
                AccountName = user.AccountName,
                AccountEmail = user.AccountEmail
            };

            if (!ModelState.IsValid)
            {
                return View("EditUser", model);
            }

            var success = _accountService.Update(
                model.AccountId,
                model.AccountName,
                model.AccountEmail,
                model.AccountRole, 
                model.AccountPassword
            );

            if (!success)
            {
                ModelState.AddModelError("", "Cập nhật thông tin thất bại.");
                return View("EditUser", model);
            }

            TempData["SuccessMessage"] = "Cập nhật thành công.";
            return RedirectToAction("EditProfile");
        }
    }
}
        