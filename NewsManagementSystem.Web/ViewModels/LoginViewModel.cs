using System.ComponentModel.DataAnnotations;

namespace NewsManagementSystem.Web.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Email không được để trống")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ")]
		public string? LoginEmail { get; set; }

		[Required(ErrorMessage = "Mật khẩu không được để trống")]
		public string? LoginPassword { get; set; }
	}
}
