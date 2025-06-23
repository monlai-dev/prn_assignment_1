using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.BusinessObject.Models;
using NewsManagementSystem.Services.Services.Abstractions;
using System.Security.Claims;

namespace NewsManagementSystem.Web.Pages.Admin
{
    [Authorize(Roles = "3")]
    public class DashboardModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly INewsService _newsService;
        private readonly ICategoryServices _categoryService;

        public int TotalUsers { get; set; }
        public int TotalNews { get; set; }
        public int TotalCategories { get; set; }

        public int ActiveArticles { get; set; }
        public int InactiveArticles { get; set; }
        public IDictionary<string, int> ArticlesByCategory { get; set; } = new Dictionary<string, int>();
        public IDictionary<DateTime, int> ArticlesByDay { get; set; } = new Dictionary<DateTime, int>();
        public List<ArticlesModel> Articles { get; set; } = new List<ArticlesModel>();

        public DashboardModel(
            IAccountService accountService,
            INewsService newsService,
            ICategoryServices categoryService)
        {
            _accountService = accountService;
            _newsService = newsService;
            _categoryService = categoryService;
        }

        public void OnGet()
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
			// Summary cards
			TotalUsers = _accountService.GetUsers(null, null).Items.Count();
            var allNews = _newsService.GetAllNewsWithDetails();
            TotalNews = allNews.Count;
            TotalCategories = _categoryService.GetAll().Count();

            // Status distribution
            ActiveArticles = allNews.Count(n => n.NewsStatus == true);
            InactiveArticles = allNews.Count(n => n.NewsStatus == false);

            // Articles by category
            ArticlesByCategory = allNews
                .GroupBy(n => n.Category?.CategoryName ?? "Uncategorized")
                .ToDictionary(g => g.Key, g => g.Count());

            // Articles by day (timeline)
            ArticlesByDay = allNews
                .Where(n => n.CreatedDate.HasValue)
                .GroupBy(n => n.CreatedDate.Value.Date)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Count());

            // Article list for table
            Articles = allNews
                .Select(n => new ArticlesModel
                {
                    Title = n.NewsTitle ?? "",
                    Category = n.Category?.CategoryName ?? "Uncategorized",
                    Author = n.CreatedBy?.AccountName ?? "Unknown",
                    CreatedDate = n.CreatedDate,
                    IsActive = n.NewsStatus == true
                })
                .OrderByDescending(a => a.CreatedDate)
                .ToList();
        }
    }
}