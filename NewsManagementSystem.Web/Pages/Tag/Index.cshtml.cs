using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.BusinessObject.Models;
using NewsManagementSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
			if (User.Identity?.IsAuthenticated ?? false)
			{
				ViewData["UserInfo"] = new LoginDTO
				{
					AccountName = User.Identity.Name,
					AccountEmail = User.Identities.FirstOrDefault()?.Claims
						.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
				};
			}
			Tag = await _context.Tags.ToListAsync();
        }
    }
}
