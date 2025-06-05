using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.DataAccess;
using NewsManagementSystem.Services.Services.Abstractions;
using NewsManagementSystem.WebMVC.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "1")]
public class NewsArticlesController : Controller
{
    private readonly INewsArticleService _service;
    private readonly IAccountService _accountService;

    public NewsArticlesController(INewsArticleService service, IAccountService accountService)
    {
        _service = service;
        _accountService = accountService;
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            var useraccount = await _accountService.FindAccountByUserName(User.Identity.Name);
            if (useraccount != null)
            {
                var userDto = new LoginDTO
                {
                    AccountName = useraccount.AccountName,
                    AccountEmail = useraccount.AccountEmail,
                };
                ViewBag.UserInfo = userDto;
            }
        }
        var articles = await _service.GetAllAsync();
        return View(articles);
    }

    public async Task<IActionResult> Details(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            var useraccount = await _accountService.FindAccountByUserName(User.Identity.Name);
            if (useraccount != null)
            {
                var userDto = new LoginDTO
                {
                    AccountName = useraccount.AccountName,
                    AccountEmail = useraccount.AccountEmail,
                };
                ViewBag.UserInfo = userDto;
            }
        }
        var article = await _service.GetByIdAsync(id);
        if (article == null) return NotFound();

        if (article.UpdatedById.HasValue)
        {
            var updater = await _accountService.GetUserById(article.UpdatedById.Value);
            ViewBag.UpdatedByName = updater?.AccountName ?? "Unknown";
        }
        else
        {
            ViewBag.UpdatedByName = "N/A";
        }


        return View(article);
    }

    public async Task<IActionResult> Create()
    {
        if (User.Identity.IsAuthenticated)
        {
            var useraccount = await _accountService.FindAccountByUserName(User.Identity.Name);
            if (useraccount != null)
            {
                var userDto = new LoginDTO
                {
                    AccountName = useraccount.AccountName,
                    AccountEmail = useraccount.AccountEmail,
                };
                ViewBag.UserInfo = userDto;
            }
        }
        var viewModel = new NewsArticleViewModel
        {
            AvailableTags = await _service.GetAllTagsAsync()
        };

        ViewData["CategoryId"] = new SelectList(await _service.GetAllCategoriesAsync(), "CategoryId", "CategoryName");

        var allAccounts = await _service.GetAllSystemAccountsAsync();
        var editorAccounts = allAccounts.Where(a => a.AccountRole == 1).ToList();

        ViewData["CreatedById"] = new SelectList(editorAccounts, "AccountId", "AccountName");
        ViewData["UpdatedById"] = new SelectList(editorAccounts, "AccountId", "AccountName");

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NewsArticleViewModel viewModel)
    {

        if (User.Identity.IsAuthenticated)
        {
            var useraccount = await _accountService.FindAccountByUserName(User.Identity.Name);
            if (useraccount != null)
            {
                var userDto = new LoginDTO
                {
                    AccountName = useraccount.AccountName,
                    AccountEmail = useraccount.AccountEmail,
                };
                ViewBag.UserInfo = userDto;
            }
        }

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
        if (User.Identity.IsAuthenticated)
        {
            var useraccount = await _accountService.FindAccountByUserName(User.Identity.Name);
            if (useraccount != null)
            {
                var userDto = new LoginDTO
                {
                    AccountName = useraccount.AccountName,
                    AccountEmail = useraccount.AccountEmail,
                };
                ViewBag.UserInfo = userDto;
            }
        }
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

        ViewData["CategoryId"] = new SelectList(await _service.GetAllCategoriesAsync(), "CategoryId", "CategoryName");

        var allAccounts = await _service.GetAllSystemAccountsAsync();
        var editorAccounts = allAccounts.Where(a => a.AccountRole == 1).ToList();

        ViewData["CreatedById"] = new SelectList(editorAccounts, "AccountId", "AccountName");
        ViewData["UpdatedById"] = new SelectList(editorAccounts, "AccountId", "AccountName");

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, NewsArticleEditViewModel viewModel)
    {
        if (User.Identity.IsAuthenticated)
        {
            var useraccount = await _accountService.FindAccountByUserName(User.Identity.Name);
            if (useraccount != null)
            {
                var userDto = new LoginDTO
                {
                    AccountName = useraccount.AccountName,
                    AccountEmail = useraccount.AccountEmail,
                };
                ViewBag.UserInfo = userDto;
            }
        }
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
        if (User.Identity.IsAuthenticated)
        {
            var useraccount = await _accountService.FindAccountByUserName(User.Identity.Name);
            if (useraccount != null)
            {
                var userDto = new LoginDTO
                {
                    AccountName = useraccount.AccountName,
                    AccountEmail = useraccount.AccountEmail,
                };
                ViewBag.UserInfo = userDto;
            }
        }
        var article = await _service.GetByIdAsync(id);
        if (article == null) return NotFound();

        return View(article);
    }

    //Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            var useraccount = await _accountService.FindAccountByUserName(User.Identity.Name);
            if (useraccount != null)
            {
                var userDto = new LoginDTO
                {
                    AccountName = useraccount.AccountName,
                    AccountEmail = useraccount.AccountEmail,
                };
                ViewBag.UserInfo = userDto;
            }
        }
        var article = await _service.GetByIdAsync(id);
        if (article == null) return NotFound();

        article.NewsStatus = false;
        await _service.UpdateAsync(article);

        return RedirectToAction(nameof(Index));
    }
}
