using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.BusinessObject.Models;
using NewsManagementSystem.DataAccess;
using NewsManagementSystem.Services.Services.Abstractions;

namespace NewsManagementSystem.Web.Pages.NewsArticles
{
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

