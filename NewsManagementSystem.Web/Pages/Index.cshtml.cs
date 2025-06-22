using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.Services.Services.Abstractions;
using System.Security.Claims;

namespace NewsManagementSystem.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly INewsService _newsService;

        public IndexModel(IAccountService accountService, INewsService newsService)
        {
            _accountService = accountService;
            _newsService = newsService;
        }

        public List<BusinessObject.Models.NewsArticle> NewsList { get; set; } = new();
        public LoginDTO? UserInfo { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int PageSize { get; set; } = 4;

        public async Task<IActionResult> OnGetAsync(int page = 1)
        {
            CurrentPage = page;

            if (User.Identity?.IsAuthenticated ?? false)
            {
                ViewData["UserInfo"] = new LoginDTO
                {
                    AccountName = User.Identity.Name,
                    AccountEmail = User.Identities.FirstOrDefault()?.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                };
            }

            var allNews = _newsService.GetAllNewsWithDetails().ToList();

            int totalNews = allNews.Count;
            TotalPages = (int)Math.Ceiling(totalNews / (double)PageSize);

            NewsList = allNews
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return Page();
        }
    }
}
