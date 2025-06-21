using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.BusinessObject.Models;
using NewsManagementSystem.DataAccess;

namespace NewsManagementSystem.Web.Pages.Tag
{
    public class DeleteModel : PageModel
    {
        private readonly FunewsManagementContext _context;

        public DeleteModel(FunewsManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                return new JsonResult(new { success = false });

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }
    }
}
