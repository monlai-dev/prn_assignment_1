    
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.BusinessObject.Models;
using NewsManagementSystem.DataAccess;
using NewsManagementSystem.Services.Services.Abstractions;
using NewsManagementSystem.Web.ViewModels.NewsArticle;
using System.Security.Claims;

namespace NewsManagementSystem.Web.Pages.NewsArticles
{
	[Authorize(Roles = "1")]
	public class CreateModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly IAccountService _accountService;
        private readonly IHubContext<DataHub> _hubContext;

        public CreateModel(
            INewsArticleService newsArticleService,
            IAccountService accountService,
            IHubContext<DataHub> hubContext)
        {
            _newsArticleService = newsArticleService;
            _accountService = accountService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticleViewModel NewsArticleVm { get; set; }

        public async Task<IActionResult> OnGetAsync()
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
			NewsArticleVm ??= new NewsArticleViewModel();
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

            var currentUser = await _accountService.FindAccountByUserName(User.Identity?.Name);
            if (currentUser == null)
                return Unauthorized();

            var selectedTags = await _newsArticleService.GetTagsByIdsAsync(NewsArticleVm.SelectedTagIds);
            int maxId = await _newsArticleService.GetMaxNewsArticleIdAsync();

            var newsArticle = new NewsArticle
            {
                NewsArticleId = (maxId + 1).ToString(),
                NewsTitle = NewsArticleVm.NewsTitle,
                Headline = NewsArticleVm.Headline,
                CreatedDate = NewsArticleVm.CreatedDate ?? DateTime.Now,
                ModifiedDate = NewsArticleVm.ModifiedDate ?? DateTime.Now,
                NewsContent = NewsArticleVm.NewsContent,
                NewsSource = NewsArticleVm.NewsSource,
                CategoryId = NewsArticleVm.CategoryId,
                NewsStatus = NewsArticleVm.NewsStatus ?? true,
                CreatedById = currentUser.AccountId,
                UpdatedById = currentUser.AccountId,
                Tags = selectedTags
            };

            await _newsArticleService.CreateAsync(newsArticle);

            await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate", "create", new
            {
                articleId = newsArticle.NewsArticleId,
                title = newsArticle.NewsTitle,
                createdBy = currentUser.AccountName,
                date = newsArticle.CreatedDate?.ToString("yyyy-MM-dd"),
				headline = newsArticle.Headline,
				category = newsArticle.Category?.CategoryName, // cần include Category khi lấy Article
				content = newsArticle.NewsContent,
				tags = selectedTags.Select(t => t.TagName).ToList()

			});

            return RedirectToPage("Index");
        }

        private async Task LoadDropdownsAsync()
        {
            NewsArticleVm ??= new NewsArticleViewModel();
            NewsArticleVm.AvailableTags = await _newsArticleService.GetAllTagsAsync();

            ViewData["CategoryId"] = new SelectList(
                await _newsArticleService.GetAllCategoriesAsync(), "CategoryId", "CategoryName");
        }
    }
}

