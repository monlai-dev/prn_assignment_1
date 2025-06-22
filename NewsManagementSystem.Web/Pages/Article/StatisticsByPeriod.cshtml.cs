using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.Web.ViewModels.NewsArticle;

namespace NewsManagementSystem.Web.Pages.Article
{
    public class StatisticsByPeriodModel : PageModel
    {
        private readonly IArticleService _articleService;

        public StatisticsByPeriodModel(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [BindProperty]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [BindProperty]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public NewsReportStatsDto NewsReportStats { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            StartDate = startDate ?? DateTime.Today.AddDays(-30);
            EndDate = endDate ?? DateTime.Today;

            await LoadStatisticsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await LoadStatisticsAsync();
            return Page();
        }

        private async Task LoadStatisticsAsync()
        {
            try
            {
                var articles = await _articleService.FindBetweenStartAndEndDateTime(StartDate.Value, EndDate.Value);

                NewsReportStats.TotalArticles = articles.Count();
                NewsReportStats.ActiveArticles = articles.Count(a => a.NewsStatus == true);
                NewsReportStats.InactiveArticles = articles.Count(a => a.NewsStatus == false);
                NewsReportStats.MostActiveAuthor = articles
                    .GroupBy(a => a.CreatedBy?.AccountName ?? "Unknown")
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault()?.Key ?? "N/A";
                NewsReportStats.ArticlesByCategory = articles
                    .GroupBy(a => a.Category?.CategoryName ?? "Uncategorized")
                    .ToDictionary(g => g.Key, g => g.Count());
                NewsReportStats.ArticlesByDay = articles
                    .Where(a => a.CreatedDate.HasValue)
                    .GroupBy(a => a.CreatedDate!.Value.Date)
                    .OrderBy(g => g.Key)
                    .ToDictionary(g => g.Key, g => g.Count());
                NewsReportStats.Articles = articles
                    .Select(a => new NewsArticleDto
                    {
                        Title = a.NewsTitle ?? "(Untitled)",
                        Category = a.Category?.CategoryName ?? "Uncategorized",
                        Author = a.CreatedBy?.AccountName ?? "Unknown",
                        CreatedDate = a.CreatedDate,
                        IsActive = a.NewsStatus == true
                    }).ToList();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while retrieving the statistics.");
            }
        }
    }
}
