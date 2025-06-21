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

namespace NewsManagementSystem.Web.Pages.NewsArticles
{
    public class DeleteModel : PageModel
    {
        private readonly FunewsManagementContext _context;
        private readonly IHubContext<DataHub> _hubContext;

        public DeleteModel(FunewsManagementContext context, IHubContext<DataHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            NewsArticle = await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(m => m.NewsArticleId == id);

            if (NewsArticle == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var article = await _context.NewsArticles.FindAsync(id);
            if (article == null) return NotFound();

            article.NewsStatus = false;
            _context.NewsArticles.Update(article);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate", "delete", new
            {
                articleId = article.NewsArticleId,
                title = article.NewsTitle,
                date = DateTime.Now.ToString("yyyy-MM-dd")
            });

            return RedirectToPage("Index");
        }
    }
}

