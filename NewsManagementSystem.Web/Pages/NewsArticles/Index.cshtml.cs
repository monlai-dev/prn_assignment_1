using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.BusinessObject.Models;
using NewsManagementSystem.DataAccess;
using NewsManagementSystem.Services.Services.Abstractions;
using NewsManagementSystem.Web.ViewModels.NewsArticle;

namespace NewsManagementSystem.Web.Pages.NewsArticles
{
    //[Authorize(Roles = "1")]
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly IAccountService _accountService;
        
        [BindProperty]
        public NewsArticleViewModel NewsArticleVm { get; set; } = new();

        public IndexModel(INewsArticleService newsArticleService, IAccountService accountService)
        {
            _newsArticleService = newsArticleService;
            _accountService = accountService;
        }

        public IEnumerable<NewsArticle> NewsArticles { get; set; }
        public LoginDTO UserInfo { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _accountService.FindAccountByUserName(User.Identity.Name);
                if (user != null)
                {
                    UserInfo = new LoginDTO
                    {
                        AccountName = user.AccountName,
                        AccountEmail = user.AccountEmail
                    };
                }
            }
            NewsArticleVm.AvailableTags = await _newsArticleService.GetAllTagsAsync();
            ViewData["CategoryId"] = new SelectList(await _newsArticleService.GetAllCategoriesAsync(), "CategoryId", "CategoryName");

            NewsArticles = await _newsArticleService.GetAllAsync();
            return Page();
        }

    }
}
