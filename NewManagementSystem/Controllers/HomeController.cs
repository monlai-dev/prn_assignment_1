using Microsoft.AspNetCore.Mvc;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.ModelsDTO;
using System.Diagnostics;

namespace NewManagementSystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAccountService _accountService;

	public HomeController(ILogger<HomeController> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
	}

	public async Task<IActionResult> IndexAsync()
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
					// Thêm các trường cần thiết nếu có
				};
				ViewBag.UserInfo = userDto;
			}
		}
		return View();
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