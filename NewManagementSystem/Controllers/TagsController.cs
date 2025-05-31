using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewManagementSystem.Models;
using NewsManagementSystem.DataAccess;
using NewsManagementSystem.Services.Services.Abstractions;

namespace NewsManagementSystem.WebMVC.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return View(tags);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var tag = await _tagService.GetTagByIdAsync(id.Value);
            if (tag == null)
                return NotFound();

            return View(tag);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TagName,Note")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                await _tagService.CreateTagAsync(tag);
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        public async Task<IActionResult> Edit(int? id)
        {
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

