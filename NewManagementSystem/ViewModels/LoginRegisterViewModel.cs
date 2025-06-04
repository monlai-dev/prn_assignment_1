using System.ComponentModel.DataAnnotations;

namespace NewsManagementSystem.WebMVC.ViewModels
{
	public class LoginRegisterViewModel
	{
		// --- Đăng Nhập ---
		[Required(ErrorMessage = "Email không được để trống")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ")]
		public string? LoginEmail { get; set; }

		[Required(ErrorMessage = "Mật khẩu không được để trống")]
		public string? LoginPassword { get; set; }

		// --- Đăng ký ---
		[Required(ErrorMessage = "Vui lòng nhập tên.")]
		public string? RegisterName { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập email.")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ.")]
		public string? RegisterEmail { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
		[DataType(DataType.Password)]
		public string? RegisterPassword { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập lại mật khẩu.")]
		[DataType(DataType.Password)]
		[Compare("RegisterPassword", ErrorMessage = "Mật khẩu không khớp.")]
		public string? ConfirmPassword { get; set; }
	}
}
