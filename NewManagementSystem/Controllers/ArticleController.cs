using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.ModelsDTO;

namespace NewManagementSystem.Controllers;

[Authorize(Roles = "3")]
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
    [HttpGet("/admin/v1/statistics")]
public async Task<IActionResult> StatisticsByPeriod(DateTime? StartDate, DateTime? EndDate)
{
    try
    {
        var start = StartDate ?? DateTime.Today.AddDays(-30);
        var end = EndDate ?? DateTime.Today;

        // 🔹 Dummy data
        var articles = await _articleService.FindBetweenStartAndEndDateTime(start, end);

        var total = articles.Count();
        var active = articles.Count(n => n.NewsStatus == true);
        var inactive = articles.Count(n => n.NewsStatus == false);

        var categoryGroup = articles
            .GroupBy(n => n.Category?.CategoryName ?? "Uncategorized")
            .ToDictionary(g => g.Key, g => g.Count());

        var dailyGroup = articles
            .GroupBy(n => n.CreatedDate!.Value.Date)
            .ToDictionary(g => g.Key, g => g.Count());

        var topAuthor = articles
            .GroupBy(n => n.CreatedBy?.AccountEmail ?? $"StaffID:{n.CreatedById}")
            .OrderByDescending(g => g.Count())
            .FirstOrDefault()?.Key ?? "Unknown";
        
        var articleDetails = articles.Select(n => new NewsArticleDetailDto
        {
            Title = n.NewsTitle ?? "(No Title)",
            Category = n.Category?.CategoryName ?? "Uncategorized",
            Author = n.CreatedBy?.AccountName ?? $"StaffID:{n.CreatedById}", // Previously was AccountEmail
            CreatedDate = n.CreatedDate,
            IsActive = n.NewsStatus == true
        }).ToList();
         
        var reportData =  new NewsReportStatsDto
        {
            TotalArticles = total,
            ActiveArticles = active,
            InactiveArticles = inactive,
            ArticlesByCategory = categoryGroup,
            ArticlesByDay = dailyGroup,
            MostActiveAuthor = topAuthor,
            Articles = articleDetails
        };
        return View("~/Views/Article/StatisticsByPeriod.cshtml", reportData);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error generating report from {Start} to {End}", StartDate, EndDate);
        TempData["ErrorMessage"] = "Failed to generate article report.";
        return View();
    }
}


}
