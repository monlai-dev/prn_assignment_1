using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.BusinessObject.Configuration
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? AccountEmail { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string? AccountPassword { get; set; }
        public string? AccountName { get; set; }
    }
}
