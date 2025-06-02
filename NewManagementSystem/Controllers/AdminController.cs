using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.ModelsDTO;
using NewsManagementSystem.Controllers.ViewModels;
using System.Linq;

namespace NewsManagementSystem.Controllers
{
	[Authorize(Roles = "3")]
	public class AdminController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAccountService accountService, ILogger<AdminController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

		// GET: /Admin/Users?role=1&email=abc@xyz.com&page=1
		public async Task<IActionResult> Users(int? role, string? email, int page = 1)
		{
			if (User.Identity.IsAuthenticated)
			{
				var user = await _accountService.FindAccountByUserName(User.Identity.Name);
				if (user != null)
				{
					var userDto = new LoginDTO
					{
						AccountName = user.AccountName,
						AccountEmail = user.AccountEmail,
					};
					ViewBag.UserInfo = userDto;
				}
			}

			var pagedResult = _accountService.GetUsers(role, email, page);
			return View(pagedResult);
		}


		// GET: /Admin/EditUser/{id}
		[Route("Admin/EditUser/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
			if (User.Identity.IsAuthenticated)
			{
				var useraccount = await _accountService.FindAccountByUserName(User.Identity.Name);
				if (useraccount != null)
				{
					var userDto = new LoginDTO
					{
						AccountName = useraccount.AccountName,
						AccountEmail = useraccount.AccountEmail,
					};
					ViewBag.UserInfo = userDto;
				}
			}
			var user = await _accountService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new UserEditViewModel
            {
                AccountId = user.AccountId,
                AccountName = user.AccountName,
                AccountEmail = user.AccountEmail,
                AccountRole = user.AccountRole
            };

            return View(viewModel);
        }

        // POST: /Admin/EditUser
        [HttpPost]
        [Route("Admin/EditUser/{id}")]
        public async Task<IActionResult> EditUser([FromRoute] short id, UserEditViewModel model)
        {
			if (User.Identity.IsAuthenticated)
			{
				var useraccount = await _accountService.FindAccountByUserName(User.Identity.Name);
				if (useraccount != null)
				{
					var userDto = new LoginDTO
					{
						AccountName = useraccount.AccountName,
						AccountEmail = useraccount.AccountEmail,
					};
					ViewBag.UserInfo = userDto;
				}
			}
			_logger.LogInformation("Model: {}", model.AccountName);
            model.AccountId = id;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Fail if Id invalid
            var success = _accountService.Update(model.AccountId, model.AccountName, model.AccountEmail, model.AccountRole, model.AccountPassword);
            if (!success)
            {
                ModelState.AddModelError("", "Failed to update user.");
                return View(model);
            }

            return RedirectToAction("Users");
        }
    }
}
