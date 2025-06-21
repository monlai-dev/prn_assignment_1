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
    public class IndexModel : PageModel
    {
        private readonly NewsManagementSystem.DataAccess.FunewsManagementContext _context;

        public IndexModel(NewsManagementSystem.DataAccess.FunewsManagementContext context)
        {
            _context = context;
        }

        public IList<BusinessObject.Models.Tag> Tag { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Tag = await _context.Tags.ToListAsync();
        }
    }
}
