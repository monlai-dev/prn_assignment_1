using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewManagementSystem.Models;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.ModelsDTO;
using NewsManagementSystem.WebMVC.ViewModels;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NewsManagementSystem.WebMVC.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[Route("/login")]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[Route("/login")]
		public async Task<IActionResult> Login(LoginRegisterViewModel model)
		{
			var login = model.Login;

			// Chỉ validate phần Login
			ModelState.Remove("Register.AccountName");
			ModelState.Remove("Register.AccountEmail");
			ModelState.Remove("Register.AccountPassword");
			ModelState.Remove("Register.ConfirmPassword");

			if (!ModelState.IsValid)
			{
				ViewBag.ShowRegister = false;
				return View("Index", model); 
			}

			var account = await _accountService.FindAccountByEmail(login.AccountEmail!);
			if (account == null)
			{
				ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng.");
				return View("Index", model);
			}

			// Hash mật khẩu người dùng nhập vào và so sánh với mật khẩu đã lưu
			string hashedInputPassword = HashPasswordWithSHA256(login.AccountPassword!);
			if (account.AccountPassword != hashedInputPassword)
			{
				ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng.");
				return View("Index", model);
			}

			// Đăng nhập thành công
			var claims = new List<Claim>
			{
				new(ClaimTypes.Sid, account.AccountId.ToString()),
				new(ClaimTypes.Name, account.AccountName),
				new(ClaimTypes.Email, account.AccountEmail),
				new(ClaimTypes.Role, account.AccountRole.ToString())
			};

			var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

			return RedirectToAction("Index", "Home");
		}


		[HttpGet]
		public IActionResult LoginGoogle()
		{
			var redirectUrl = Url.Action("ResponseGoogle", "Account");
			var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
			return Challenge(properties, GoogleDefaults.AuthenticationScheme);

		}

		[HttpGet]
		public async Task<IActionResult> ResponseGoogle()
		{
			var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			var account = new SystemAccount();
			if (!authenticateResult.Succeeded || authenticateResult.Principal == null)
			{
				return RedirectToAction("Index", "Home");
			}
			var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims.Select(claim => new
			{
				claim.Type,
				claim.Value
			});
			var userId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			var userName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
			var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
			if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
			{
				return RedirectToAction("Index", "Home");
			}
			var existingUser = _accountService.GetByEmail(email);
			if (existingUser == null)
			{
				account.AccountEmail = email;
				account.AccountName = userName;
				account.AccountRole = 1; 
				await _accountService.CreateAccount(account);
			}
			else
			{
				account = existingUser;
			}
			var claimsIdentity = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.NameIdentifier, userId),
					new Claim(ClaimTypes.Sid , account.AccountId.ToString()),
					new Claim(ClaimTypes.Name, userName ?? ""),
					new Claim(ClaimTypes.Email, email ?? ""),
					new Claim(ClaimTypes.Role, account.AccountRole.ToString())
				}, CookieAuthenticationDefaults.AuthenticationScheme);

			var principal = new ClaimsPrincipal(claimsIdentity);
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		[Route("/register")]
		public async Task<IActionResult> Register(LoginRegisterViewModel model)
		{
			var dto = model.Register;

			// Chỉ validate phần Register
			ModelState.Remove("Login.AccountEmail");
			ModelState.Remove("Login.AccountPassword");

			if (!ModelState.IsValid)
			{
				ViewBag.ShowRegister = true;
				return View("Index", model); 
			}

			// Kiểm tra email đã tồn tại chưa
			var existingEmail = await _accountService.FindAccountByEmail(dto.AccountEmail!);
			if (existingEmail != null)
			{
				ViewBag.MessageRegister = "Email đã tồn tại. Vui lòng chọn email khác!";
				ViewBag.ShowRegister = true;
				return View("Index", model);
			}

			// Hash password
			string hashedPassword = HashPasswordWithSHA256(dto.AccountPassword!);

			// Tạo object tài khoản mới
			var account = new SystemAccount
			{
				AccountName = dto.AccountName!,
				AccountEmail = dto.AccountEmail!,
				AccountPassword = hashedPassword,
				AccountRole = 1  // User role mặc định
			};

			try
			{
				// Gọi service tạo tài khoản mới
				await _accountService.CreateAccount(account);

				// Thành công, thông báo đăng ký thành công và redirect về login
				TempData["LOGIN_ERROR"] = "Đăng ký thành công! Mời bạn đăng nhập.";
				return RedirectToAction("Index", "Account");
			}
			catch (Exception ex)
			{
				ViewBag.MessageRegister = "Đăng ký thất bại, vui lòng thử lại!";
				ViewBag.ShowRegister = true;
				return View("Index", model);
			}
		}


		public string HashPasswordWithSHA256(string password)
		{
			using (var sha256 = SHA256.Create())
			{
				var bytes = Encoding.UTF8.GetBytes(password);
				var hash = sha256.ComputeHash(bytes);
				return Convert.ToHexString(hash); // chuỗi hex 64 ký tự
			}
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}
	}
}
