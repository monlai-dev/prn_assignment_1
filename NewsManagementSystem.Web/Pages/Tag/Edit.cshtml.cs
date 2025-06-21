using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.BusinessObject.Models;
using NewsManagementSystem.DataAccess;

namespace NewsManagementSystem.Web.Pages.Tag
{
    public class EditModel : PageModel
    {
        private readonly FunewsManagementContext _context;

        public EditModel(FunewsManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BusinessObject.Models.Tag Tag { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return NotFound();

            return new JsonResult(new
            {
                tagId = tag.TagId,
                tagName = tag.TagName,
                note = tag.Note
            });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var tagInDb = await _context.Tags.FindAsync(Tag.TagId);
            if (tagInDb == null)
                return new JsonResult(new { success = false });

            tagInDb.TagName = Tag.TagName;
            tagInDb.Note = Tag.Note;

            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                success = true,
                tagId = tagInDb.TagId,
                tagName = tagInDb.TagName,
                note = tagInDb.Note
            });
        }
    }
}
