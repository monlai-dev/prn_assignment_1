
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.Services.Services.Abstractions;
using NewsManagementSystem.Web.ViewModels.NewsArticle;

namespace NewsManagementSystem.Web.Pages.NewsArticles
{
    public class EditModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly IAccountService _accountService;
        private readonly IHubContext<DataHub> _hubContext;

        public EditModel(INewsArticleService newsArticleService, IAccountService accountService, IHubContext<DataHub> hubContext)
        {
            _newsArticleService = newsArticleService;
            _accountService = accountService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticleEditViewModel NewsArticleVm { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var article = await _newsArticleService.GetByIdAsync(id);
            if (article == null) return NotFound();

            var currentUser = await _accountService.FindAccountByUserName(User.Identity?.Name);
            if (currentUser == null || article.CreatedById != currentUser.AccountId)
            {
                return Forbid();
            }

            NewsArticleVm = new NewsArticleEditViewModel
            {
                NewsArticleId = article.NewsArticleId,
                NewsTitle = article.NewsTitle,
                Headline = article.Headline,
                CreatedDate = article.CreatedDate,
                NewsContent = article.NewsContent,
                NewsSource = article.NewsSource,
                CategoryId = article.CategoryId,
                NewsStatus = article.NewsStatus,
                ModifiedDate = article.ModifiedDate,
                SelectedTagIds = article.Tags.Select(t => t.TagId).ToList()
            };

            await LoadDropdownsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return Page();
            }

            var articleToUpdate = await _newsArticleService.GetByIdAsync(int.Parse(NewsArticleVm.NewsArticleId.ToString()));
            if (articleToUpdate == null) return NotFound();

            var currentUser = await _accountService.FindAccountByUserName(User.Identity?.Name);
            if (currentUser == null || articleToUpdate.CreatedById != currentUser.AccountId)
            {
                return Forbid();
            }

            var tags = await _newsArticleService.GetTagsByIdsAsync(NewsArticleVm.SelectedTagIds);

            articleToUpdate.NewsTitle = NewsArticleVm.NewsTitle;
            articleToUpdate.Headline = NewsArticleVm.Headline;
            articleToUpdate.CreatedDate = NewsArticleVm.CreatedDate ?? DateTime.Now;
            articleToUpdate.NewsContent = NewsArticleVm.NewsContent;
            articleToUpdate.NewsSource = NewsArticleVm.NewsSource;
            articleToUpdate.CategoryId = NewsArticleVm.CategoryId;
            articleToUpdate.NewsStatus = NewsArticleVm.NewsStatus;
            articleToUpdate.ModifiedDate = NewsArticleVm.ModifiedDate ?? DateTime.Now;
            articleToUpdate.Tags = tags;

            await _newsArticleService.UpdateAsync(articleToUpdate);

            // 🔔 SignalR: Notify article update
            await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate", "update", new
            {
                articleId = articleToUpdate.NewsArticleId,
                title = articleToUpdate.NewsTitle,
                updatedAt = articleToUpdate.ModifiedDate?.ToString("yyyy-MM-dd HH:mm")
            });

            return RedirectToPage("Index");
        }

        private async Task LoadDropdownsAsync()
        {
            NewsArticleVm.AvailableTags = await _newsArticleService.GetAllTagsAsync();

            var categories = await _newsArticleService.GetAllCategoriesAsync();
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
        }
    }
}

