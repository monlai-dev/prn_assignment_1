using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.Services.Services.Abstractions;

namespace NewsManagementSystem.WebMVC.Controllers
{
	[Authorize(Roles = "1")]
	public class CategoryController : Controller
    {
        private readonly ICategoryServices _service;
        private readonly IAccountService _accountService;

		public CategoryController(ICategoryServices service, IAccountService accountService)
        {
            _service = service;
            _accountService = accountService;
		}

        public async Task<IActionResult> Index()
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
			var categories = _service.GetAll();
            return View(categories);
        }

        public async Task<IActionResult> Details(short id)
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
			var category = _service.GetById(id);
            if (category == null) return NotFound();
            return View(category);
        }

        public async Task<IActionResult> Create()
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
			var category = new Category(); // Initialize a new Category object
            ViewData["ParentCategoryId"] = new SelectList(_service.GetAll(), "CategoryId", "CategoryName");
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
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
			if (ModelState.IsValid)
            {
                _service.Add(category);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentCategoryId"] = new SelectList(_service.GetAll(), "CategoryId", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        public async Task<IActionResult> Edit(short id)
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
			var category = _service.GetById(id);
            if (category == null) return NotFound();
            ViewData["ParentCategoryId"] = new SelectList(_service.GetAll(), "CategoryId", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
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
			if (ModelState.IsValid)
            {
                _service.Update(category);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentCategoryId"] = new SelectList(_service.GetAll(), "CategoryId", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        public async Task<IActionResult> Delete(short id)
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
			var category = _service.GetById(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
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
			bool success = _service.Delete(id);
            if (!success)
            {
                TempData["Error"] = "Cannot delete this category. It is being used in news articles.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
