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
    public class DetailsModel : PageModel
    {
        private readonly NewsManagementSystem.DataAccess.FunewsManagementContext _context;

        public DetailsModel(NewsManagementSystem.DataAccess.FunewsManagementContext context)
        {
            _context = context;
        }

        public BusinessObject.Models.Tag Tag { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags.FirstOrDefaultAsync(m => m.TagId == id);
            if (tag == null)
            {
                return NotFound();
            }
            else
            {
                Tag = tag;
            }
            return Page();
        }
    }
}
