using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.Services.Services.Abstractions;

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

		public List<NewsArticle> NewsList { get; set; } = new();
		public LoginDTO? UserInfo { get; set; }
		public int CurrentPage { get; set; } = 1;
		public int TotalPages { get; set; } = 1;
		public int PageSize { get; set; } = 4;

		public async Task<IActionResult> OnGetAsync(int page = 1)
		{
			CurrentPage = page;

			if (User.Identity?.IsAuthenticated ?? false)
			{
				var user = await _accountService.FindAccountByUserName(User.Identity.Name);
				ViewData["UserInfo"] = new LoginDTO
				{
					AccountName = user.AccountName,
					AccountEmail = user.AccountEmail
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
