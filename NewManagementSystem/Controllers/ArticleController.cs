using Microsoft.AspNetCore.Mvc;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;

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

    // GET: Article/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("Invalid article ID");
            }

            // You'll need to implement GetByIdAsync in your service
            // var article = await _articleService.GetByIdAsync(id);
            
            // For now, just return a view with the ID
            ViewBag.ArticleId = id;
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving article with ID: {ArticleId}", id);
            TempData["ErrorMessage"] = "Article not found.";
            return RedirectToAction("Index");
        }
    }

    // GET: Article/Search?query=news&category=tech
    public async Task<IActionResult> Search(string query, string category = "")
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                ViewBag.Message = "Please enter a search term.";
                return View();
            }

            ViewBag.Query = query;
            ViewBag.Category = category;
            
            // Implement search logic here
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching articles with query: {Query}", query);
            TempData["ErrorMessage"] = "Search failed.";
            return View();
        }
    }

    // GET: Article/ByDate?date=2025-05-29
    public async Task<IActionResult> ByDate(DateTime? StartDate, DateTime? EndDate)
    {
        try
        {
            var searchDate = StartDate ?? DateTime.Today;
            var nextDay = EndDate ?? searchDate.AddDays(1);

            var articles = await _articleService.FindBetweenStartAndEndDateTime(searchDate, nextDay);
            
            ViewBag.SearchDate = searchDate.ToString("yyyy-MM-dd");
            ViewBag.ArticleCount = articles.Count();
            
            return View(articles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving articles for date: {Date}", StartDate);
            TempData["ErrorMessage"] = "Failed to load articles.";
            return View(new List<NewsArticle>());
        }
    }
}
