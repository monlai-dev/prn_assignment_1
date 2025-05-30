using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.BusinessObject.ModelsDTO
{
	public class RegisterDTO
	{

		[Required(ErrorMessage = "Vui lòng nhập tên.")]
		public string? AccountName { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập email.")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ.")]
		public string? AccountEmail { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
		[DataType(DataType.Password)]
		public string? AccountPassword { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập lại mật khẩu.")]
		[DataType(DataType.Password)]
		[Compare("AccountPassword", ErrorMessage = "Mật khẩu không khớp.")]
		public string? ConfirmPassword { get; set; }
	}
}
