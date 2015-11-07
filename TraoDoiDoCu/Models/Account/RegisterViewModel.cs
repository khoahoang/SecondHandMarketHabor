using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TraoDoiDoCu.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Tên tài khoản ít nhất 6 kí tự và nhiều nhất 25 kí tự.")]
        [Display(Name = "Tên tài khoản:")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu ít nhất 6 kí tự và nhiều nhất 50 kí tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không trùng nhau.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Họ nhiều hơn 1 kí tự và ít hơn 50 kí tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Họ")]
        public string LastName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Tên nhiều hơn 2 kí tự và ít hơn 50 kí tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Tên")]
        public string FirstName { get; set; }

        [StringLength(15, MinimumLength = 6, ErrorMessage = "Số điện thoại nhiều hơn 5 kí tự và ít hơn 16 kí tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }
    }
}