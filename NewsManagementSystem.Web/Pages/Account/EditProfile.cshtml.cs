using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewManagementSystem.Services.Abstractions;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.Services.Services.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace NewsManagementSystem.Web.Pages.Account
{
	[Authorize(Roles = "4")]
	[Authorize(Roles = "2")]
	public class EditProfileModel : PageModel
    {
        private readonly IAccountService _accountService;

        public EditProfileModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty] public EditProfileViewModel EditProfile { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is authenticated
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToPage("/Account/LoginRegister");
            }

            // Get current user info from authentication system
            var currentUser = await _accountService.FindAccountByUserName(User.Identity.Name);
            if (currentUser == null)
            {
                return RedirectToPage("/Account/LoginRegister");
            }

            // Populate the form with current user data
            EditProfile.AccountName = currentUser.AccountName;
            EditProfile.AccountEmail = currentUser.AccountEmail;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Check if user is authenticated
                if (!User.Identity?.IsAuthenticated ?? true)
                {
                    TempData["ErrorMessage"] = "User session not found. Please log in again.";
                    return RedirectToPage("/Account/LoginRegister");
                }

                // Get current user info from authentication system
                var currentUser = await _accountService.FindAccountByUserName(User.Identity.Name);
                if (currentUser == null)
                {
                    TempData["ErrorMessage"] = "User not found. Please log in again.";
                    return RedirectToPage("/Account/LoginRegister");
                }

                // Validate new password confirmation
                if (!string.IsNullOrEmpty(EditProfile.NewPassword))
                {
                    if (EditProfile.NewPassword != EditProfile.ConfirmPassword)
                    {
                        ModelState.AddModelError("EditProfile.ConfirmPassword",
                            "New password and confirmation password do not match.");
                        return Page();
                    }
                }

                // Use service to update profile
                var (success, message) = await _accountService.UpdateProfileAsync(
                    currentUser.AccountName,
                    EditProfile.AccountName,
                    EditProfile.AccountEmail,
                    EditProfile.CurrentPassword,
                    EditProfile.NewPassword
                );

                if (success)
                {
                    TempData["SuccessMessage"] = message;
                    return RedirectToPage("/index");
                }
                else
                {
                    // Handle specific validation errors
                    if (message.Contains("Current password is required"))
                    {
                        ModelState.AddModelError("EditProfile.CurrentPassword", message);
                    }
                    else if (message.Contains("Current password is incorrect"))
                    {
                        ModelState.AddModelError("EditProfile.CurrentPassword", message);
                    }
                    else if (message.Contains("Username is already taken"))
                    {
                        ModelState.AddModelError("EditProfile.AccountName", message);
                    }
                    else if (message.Contains("Email is already registered"))
                    {
                        ModelState.AddModelError("EditProfile.AccountEmail", message);
                    }
                    else if (message.Contains("Password must be at least"))
                    {
                        ModelState.AddModelError("EditProfile.NewPassword", message);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = message;
                    }

                    return Page();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating your profile. Please try again.";
                return Page();
            }
        }
        
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(hash);
        }
    }

    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        [Display(Name = "Username")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string AccountEmail { get; set; } = string.Empty;

        [Display(Name = "Current Password")] public string? CurrentPassword { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Password and confirmation password do not match")]
        [Display(Name = "Confirm New Password")]
        public string? ConfirmPassword { get; set; }
    }

    
}