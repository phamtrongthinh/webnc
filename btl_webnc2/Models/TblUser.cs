using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace btl_webnc2.Models;

public partial class TblUser
{
    [Key]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
    [StringLength(50, ErrorMessage = "Tên tài khoản không được vượt quá 50 ký tự")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
     public string? Email { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]    
    public string? Password { get; set; }
    [Required(ErrorMessage = "Vui lòng chọn vai trò")]
    public string? Role { get; set; }
}
