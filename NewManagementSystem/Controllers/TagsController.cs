using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.ModelsDTO;
using NewsManagementSystem.DataAccess;
using NewsManagementSystem.Services.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsManagementSystem.WebMVC.Controllers
{
	[Authorize(Roles = "2")]
	public class TagsController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IAccountService _accountService;

		public TagsController(ITagService tagService, IAccountService accountService)
        {
            _tagService = tagService;
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
			var tags = await _tagService.GetAllTagsAsync();
            return View(tags);
        }

        public async Task<IActionResult> Details(int? id)
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
			if (id == null)
                return NotFound();

            var tag = await _tagService.GetTagByIdAsync(id.Value);
            if (tag == null)
                return NotFound();

            return View(tag);
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
			return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TagName,Note")] Tag tag)
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
                await _tagService.CreateTagAsync(tag);
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        public async Task<IActionResult> Edit(int? id)
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
			if (id == null)
                return NotFound();

            var tag = await _tagService.GetTagByIdAsync(id.Value);
            if (tag == null)
                return NotFound();

            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TagId,TagName,Note")] Tag tag)
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
			if (id != tag.TagId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _tagService.UpdateTagAsync(tag);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _tagService.IsTagInUseAsync(tag.TagId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        public async Task<IActionResult> Delete(int? id)
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
			if (id == null)
                return NotFound();

            var tag = await _tagService.GetTagByIdAsync(id.Value);
            if (tag == null)
                return NotFound();

            return View(tag);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
			var isUsed = await _tagService.IsTagInUseAsync(id); 

            if (isUsed)
            {
                TempData["ErrorMessage"] = "Không thể xóa Tag vì đang được sử dụng trong bài viết.";
                return RedirectToAction(nameof(Delete), new { id }); 
            }

            await _tagService.DeleteTagAsync(id); 
            TempData["SuccessMessage"] = "Tag đã được xóa thành công.";
            return RedirectToAction(nameof(Index));
        }



    }
}

