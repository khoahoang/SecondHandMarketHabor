using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TraoDoiDoCu.Models.Account
{
    public class LoginViewModel
    {       
        [Required]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Tên tài khoản nhiều hơn 5 kí tự và ít hơn 25 kí tự.")]
        [Display(Name = "Tên tài khoản:")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu nhiều hơn 5 kí tự và ít hơn 50 kí tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
    }
}