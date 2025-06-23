using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.BusinessObject.Models;
using NewsManagementSystem.DataAccess;
using NewsManagementSystem.Services.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NewsManagementSystem.Web.Pages.NewsArticles
{
	[Authorize(Roles = "1")]
	public class DeleteModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly IHubContext<DataHub> _hubContext;

        public DeleteModel(INewsArticleService newsArticleService, IHubContext<DataHub> hubContext)
        {
            _newsArticleService = newsArticleService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
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
			if (string.IsNullOrEmpty(id)) return NotFound();

            NewsArticle = await _newsArticleService.GetByIdAsync(int.Parse(id));
            if (NewsArticle == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            await _newsArticleService.DeleteAsync(int.Parse(id));

            await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate", "delete", new
            {
                articleId = id,
                title = NewsArticle?.NewsTitle ?? "Unknown",
                date = DateTime.Now.ToString("yyyy-MM-dd")
            });

            return RedirectToPage("Index");
        }
    }
}

