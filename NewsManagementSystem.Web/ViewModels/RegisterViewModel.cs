using System.ComponentModel.DataAnnotations;

namespace NewsManagementSystem.Web.ViewModels
{
	public class RegisterViewModel
	{
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
