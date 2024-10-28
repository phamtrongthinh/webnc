﻿using System.ComponentModel.DataAnnotations;

namespace btl_webnc2.Models
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string? Password { get; set; }
      
    }
}
