using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.Web.ViewModels;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;

namespace NewsManagementSystem.Web.Pages.Account
{
	public class LoginRegisterModel : PageModel
	{
		private readonly IAccountService _accountService;

		public LoginRegisterModel(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[BindProperty]
		public LoginViewModel Login { get; set; }

		[BindProperty]
		public RegisterViewModel Register { get; set; }

		public IActionResult OnGet()
		{
			return Page();
		}

	public async Task<IActionResult> OnPostLoginAsync()
	{
		// Chỉ validate Login — bỏ qua các field của Register
		ModelState.Remove("Register.RegisterName");
		ModelState.Remove("Register.RegisterEmail");
		ModelState.Remove("Register.RegisterPassword");
		ModelState.Remove("Register.ConfirmPassword");

		SystemAccount account;

		if (_accountService.IsAdminLogin(Login.LoginEmail!, Login.LoginPassword!))
		{
			account = new SystemAccount
			{
				AccountRole = 3, // Admin role
				AccountEmail = Login.LoginEmail!,
				AccountName = "Literally Admin",
				AccountId = 6969
			};
		}
		else
		{
			account = await _accountService.FindAccountByEmail(Login.LoginEmail!);

			if (account == null || account.AccountPassword != HashPassword(Login.LoginPassword!))
			{
				ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng.");
				ViewData["ShowRegister"] = false;
				return Page();
			}
		}

		// Đăng nhập thành công
		var claims = new List<Claim>
			{
				new(ClaimTypes.Sid, account.AccountId.ToString()),
				new(ClaimTypes.Name, account.AccountName ?? ""),
				new(ClaimTypes.Email, account.AccountEmail),
				new(ClaimTypes.Role, account.AccountRole.ToString())
			};
		var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
		await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

		// Phân quyền redirect
		return account.AccountRole switch
		{
			1 => RedirectToPage("/NewsArticles/Index"),
			2 => RedirectToPage("/Index"), // Home
			3 => RedirectToPage("/Admin/Index"),
			_ => RedirectToPage("/Index")
		};
	}

		public async Task<IActionResult> OnPostRegisterAsync()
		{
			var existing = await _accountService.FindAccountByEmail(Register.RegisterEmail!);
			if (existing != null)
			{
				ModelState.AddModelError(string.Empty, "Email đã tồn tại.");
				ViewData["ShowRegister"] = true;
				return Page();
			}

			var account = new SystemAccount
			{
				AccountName = Register.RegisterName!,
				AccountEmail = Register.RegisterEmail!,
				AccountPassword = HashPassword(Register.RegisterPassword!),
				AccountRole = 4
			};

			try
			{
				await _accountService.CreateAccount(account);
				TempData["LOGIN_ERROR"] = "Đăng ký thành công! Mời bạn đăng nhập.";
				return RedirectToPage("/Account/LoginRegister");
			}
			catch
			{
				ModelState.AddModelError(string.Empty, "Đăng ký thất bại, vui lòng thử lại.");
				ViewData["ShowRegister"] = true;
				return Page();
			}
		}

		public IActionResult OnGetLoginGoogle()
		{
			var redirectUrl = Url.Page("LoginRegister", "GoogleResponse");
			var props = new AuthenticationProperties { RedirectUri = redirectUrl };
			return Challenge(props, GoogleDefaults.AuthenticationScheme);
		}

		public async Task<IActionResult> OnGetGoogleResponseAsync()
		{
			var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			if (!result.Succeeded || result.Principal == null)
				return RedirectToPage("/Index");

			var claims = result.Principal.Claims;
			var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
			var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
   
			if (string.IsNullOrEmpty(email)) return RedirectToPage("/Index");

			var account = _accountService.GetByEmail(email) ?? new SystemAccount
			{
				AccountEmail = email,
				AccountName = name,
				AccountRole = 4
			};

			if (account.AccountId == 0)
				await _accountService.CreateAccount(account);

			await SignInAsync(account);
			return RedirectToPage("/Index");
		}

		public async Task<IActionResult> OnGetLogoutAsync()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToPage("/Account/LoginRegister");
		}

		private string HashPassword(string password)
		{
			using var sha = SHA256.Create();
			var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Convert.ToHexString(hash);
		}

		private async Task SignInAsync(SystemAccount account)
		{
			var claims = new List<Claim>
		{
			new(ClaimTypes.Sid, account.AccountId.ToString()),
			new(ClaimTypes.Name, account.AccountName ?? ""),
			new(ClaimTypes.Email, account.AccountEmail),
			new(ClaimTypes.Role, account.AccountRole.ToString())
		};

			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
		}
	}
}
