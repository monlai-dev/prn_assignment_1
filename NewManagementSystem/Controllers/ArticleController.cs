using Microsoft.AspNetCore.Mvc;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.ModelsDTO;

namespace NewManagementSystem.Controllers;

public class ArticleController : Controller
{
    private readonly IArticleService _articleService;
    private readonly ILogger<ArticleController> _logger;

    public ArticleController(IArticleService articleService, ILogger<ArticleController> logger)
    {
        _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    

    

    // GET: Article/Statistics?StartDate=2025-05-01&EndDate=2025-05-29
    [HttpGet("/v1/statistics")]
public async Task<IActionResult> StatisticsByPeriod(DateTime? StartDate, DateTime? EndDate)
{
    try
    {
        var start = StartDate ?? DateTime.Today.AddDays(-30);
        var end = EndDate ?? DateTime.Today;

        // 🔹 Dummy data
        var articles = new List<NewsArticleReportViewModel>
        {
            new NewsArticleReportViewModel
            {
                NewsArticleId = "1",
                NewsTitle = "Tech Innovations in 2025",
                Headline = "AI leads the way",
                CreatedDate = new DateTime(2025, 5, 10),
                NewsSource = "TechTimes",
                
            },
            new NewsArticleReportViewModel
            {
                NewsArticleId = "2",
                NewsTitle = "Global Markets Update",
                Headline = "Stocks rise after Fed announcement",
                CreatedDate = new DateTime(2025, 5, 15),
                NewsSource = "FinanceDaily",
                
            },
            new NewsArticleReportViewModel
            {
                NewsArticleId = "3",
                NewsTitle = "Health Breakthroughs",
                Headline = "New cancer treatment approved",
                CreatedDate = new DateTime(2025, 5, 20),
                NewsSource = "HealthWorld",
                
            }
        };

        var reportData = articles
            .Where(a => a.CreatedDate >= start && a.CreatedDate <= end)
            .OrderByDescending(a => a.CreatedDate)
            .Select(a => new NewsArticleReportViewModel
            {
                NewsArticleId = a.NewsArticleId,
                NewsTitle = a.NewsTitle,
                Headline = a.Headline,
                CreatedDate = a.CreatedDate,
                NewsSource = a.NewsSource,
            })
            .ToList();

        ViewBag.TotalArticles = reportData.Count;
        ViewBag.StartDate = start.ToString("yyyy-MM-dd");
        ViewBag.EndDate = end.ToString("yyyy-MM-dd");

        return View("~/Views/Article/StatisticsByPeriod.cshtml", reportData);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error generating report from {Start} to {End}", StartDate, EndDate);
        TempData["ErrorMessage"] = "Failed to generate article report.";
        return View(new List<NewsArticleReportViewModel>());
    }
}


}
