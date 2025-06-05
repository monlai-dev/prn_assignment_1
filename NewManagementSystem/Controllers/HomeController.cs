using Microsoft.AspNetCore.Mvc;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.Services.Services.Abstractions;
using System.Diagnostics;

namespace NewManagementSystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAccountService _accountService;
	private readonly INewsService _newsService;

	public HomeController(ILogger<HomeController> logger, IAccountService accountService, INewsService newsService)
    {
        _logger = logger;
        _accountService = accountService;
		_newsService = newsService;
	}

	public async Task<IActionResult> IndexAsync(int page = 1)
	{
		if (User.Identity.IsAuthenticated)
		{
			var user = await _accountService.FindAccountByUserName(User.Identity.Name);
			if (user != null)
			{
				var userDto = new LoginDTO
				{
					AccountName = user.AccountName,
					AccountEmail = user.AccountEmail,
				};
				ViewBag.UserInfo = userDto;
			}
		}

		const int pageSize = 4;
		var allNews = _newsService.GetAllNewsWithDetails().ToList();

		int totalNews = allNews.Count;
		int totalPages = (int)Math.Ceiling(totalNews / (double)pageSize);

		var newsToShow = allNews
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.ToList();

		ViewBag.CurrentPage = page;
		ViewBag.TotalPages = totalPages;

		return View(newsToShow);
	}



	public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}