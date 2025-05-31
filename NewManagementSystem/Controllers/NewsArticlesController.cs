using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewManagementSystem.Models;
using NewsManagementSystem.DataAccess;
using NewsManagementSystem.Services.Services.Abstractions;
using NewsManagementSystem.WebMVC.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using System;

public class NewsArticlesController : Controller
{
    private readonly INewsArticleService _service;

    public NewsArticlesController(INewsArticleService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var articles = await _service.GetAllAsync();
        return View(articles);
    }

    public async Task<IActionResult> Details(int id)
    {
        var article = await _service.GetByIdAsync(id);
        if (article == null) return NotFound();

        return View(article);
    }

    public async Task<IActionResult> Create()
    {
        var viewModel = new NewsArticleViewModel
        {
            AvailableTags = await _service.GetAllTagsAsync()
        };

        ViewData["CategoryId"] = new SelectList(await _service.GetAllCategoriesAsync(), "CategoryId", "CategoryDesciption");
        ViewData["CreatedById"] = new SelectList(await _service.GetAllSystemAccountsAsync(), "AccountId", "AccountId");

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NewsArticleViewModel viewModel)
    {

       

        if (ModelState.IsValid)
        {
            var selectedTags = await _service.GetTagsByIdsAsync(viewModel.SelectedTagIds);

            int maxId = await _service.GetMaxNewsArticleIdAsync();
            string newId = (maxId + 1).ToString();

            var newsArticle = new NewsArticle
            {
                NewsArticleId = newId,
                NewsTitle = viewModel.NewsTitle,
                Headline = viewModel.Headline,
                CreatedDate = viewModel.CreatedDate ?? DateTime.Now,
                NewsContent = viewModel.NewsContent,
                NewsSource = viewModel.NewsSource,
                CategoryId = viewModel.CategoryId,
                NewsStatus = viewModel.NewsStatus ?? true,
                CreatedById = viewModel.CreatedById,
                UpdatedById = viewModel.UpdatedById,
                ModifiedDate = viewModel.ModifiedDate ?? DateTime.Now,
                Tags = selectedTags
            };

            await _service.CreateAsync(newsArticle);
            return RedirectToAction(nameof(Index));
        }

        viewModel.AvailableTags = await _service.GetAllTagsAsync();
        ViewData["CategoryId"] = new SelectList(await _service.GetAllCategoriesAsync(), "CategoryId", "CategoryDesciption", viewModel.CategoryId);
        ViewData["CreatedById"] = new SelectList(await _service.GetAllSystemAccountsAsync(), "AccountId", "AccountId", viewModel.CreatedById);

        return View(viewModel);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var article = await _service.GetByIdAsync(id);
        if (article == null) return NotFound();

        var viewModel = new NewsArticleEditViewModel
        {
            NewsArticleId = article.NewsArticleId,
            NewsTitle = article.NewsTitle,
            Headline = article.Headline,
            CreatedDate = article.CreatedDate,
            NewsContent = article.NewsContent,
            NewsSource = article.NewsSource,
            CategoryId = article.CategoryId,
            NewsStatus = article.NewsStatus,
            CreatedById = article.CreatedById,
            UpdatedById = article.UpdatedById,
            ModifiedDate = article.ModifiedDate,
            SelectedTagIds = article.Tags.Select(t => t.TagId).ToList(),
            AvailableTags = await _service.GetAllTagsAsync()
        };

        ViewData["CategoryId"] = new SelectList(await _service.GetAllCategoriesAsync(), "CategoryId", "CategoryDesciption", viewModel.CategoryId);
        ViewData["CreatedById"] = new SelectList(await _service.GetAllSystemAccountsAsync(), "AccountId", "AccountId", viewModel.CreatedById);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, NewsArticleEditViewModel viewModel)
    {
        if (id.ToString() != viewModel.NewsArticleId) return NotFound();

        if (ModelState.IsValid)
        {
            var existingArticle = await _service.GetByIdAsync(id);
            if (existingArticle == null) return NotFound();

            existingArticle.NewsTitle = viewModel.NewsTitle;
            existingArticle.Headline = viewModel.Headline;
            existingArticle.CreatedDate = viewModel.CreatedDate;
            existingArticle.NewsContent = viewModel.NewsContent;
            existingArticle.NewsSource = viewModel.NewsSource;
            existingArticle.CategoryId = viewModel.CategoryId;
            existingArticle.NewsStatus = viewModel.NewsStatus;
            existingArticle.CreatedById = viewModel.CreatedById;
            existingArticle.UpdatedById = viewModel.UpdatedById;
            existingArticle.ModifiedDate = viewModel.ModifiedDate;

            var selectedTags = await _service.GetTagsByIdsAsync(viewModel.SelectedTagIds);
            existingArticle.Tags = selectedTags;

            await _service.UpdateAsync(existingArticle);
            return RedirectToAction(nameof(Index));
        }

        viewModel.AvailableTags = await _service.GetAllTagsAsync();
        ViewData["CategoryId"] = new SelectList(await _service.GetAllCategoriesAsync(), "CategoryId", "CategoryDesciption", viewModel.CategoryId);
        ViewData["CreatedById"] = new SelectList(await _service.GetAllSystemAccountsAsync(), "AccountId", "AccountId", viewModel.CreatedById);

        return View(viewModel);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var article = await _service.GetByIdAsync(id);
        if (article == null) return NotFound();

        return View(article);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var article = await _service.GetByIdAsync(id);
        if (article == null) return NotFound();

        article.NewsStatus = false;
        await _service.UpdateAsync(article);

        return RedirectToAction(nameof(Index));
    }
}
