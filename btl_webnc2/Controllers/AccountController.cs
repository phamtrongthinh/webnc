using btl_webnc2.Data;
using btl_webnc2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace btl_webnc2.Controllers
{
    public class AccountController : Controller
    {
        private readonly QuanLyLopHocTrucTuyen2Context _context;
        public AccountController(QuanLyLopHocTrucTuyen2Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                // Kiểm tra model hợp lệ
                if (!ModelState.IsValid) {
                    return View(model);
                }
                // Kiểm tra người dùng với email và mật khẩu
                var user = await _context.TblUsers
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Email hoặc mật khẩu không chính xác");
                    return View(model);
                }

                // Lưu thông tin vào session
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("UserName", user.Username);

                // Chuyển hướng theo role
                if (user.Role == "Student") return RedirectToAction("Student", "Home");
                if (user.Role == "Teacher") return RedirectToAction("Teacher", "Home");
                if (user.Role == "Admin") return RedirectToAction("Admin", "Home");

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "Có lỗi xảy ra khi đăng nhập. Vui lòng thử lại sau.");
                return View();
            }
        
        }


        [HttpGet]
        public IActionResult Register()
        {
            // Nếu TempData có thông báo thành công, gửi nó sang View để hiển thị
            ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            return View();
        }

        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);

                }
                var existingUser = await _context.TblUsers.FirstOrDefaultAsync(x => x.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng");
                    return View(model);
                }
                //lấy thông tin user đó
                var newUser = new TblUser
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    Role = model.Role,
                };
                _context.TblUsers.Add(newUser);
                await _context.SaveChangesAsync();
                // Lưu thông báo thành công vào TempData
                TempData["SuccessMessage"] = "Đăng ký thành công! Bạn có thể tạo tài khoản mới.";

                // Redirect về lại trang đăng ký để xóa ModelState và dữ liệu cũ
                return RedirectToAction("Register");
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                ModelState.AddModelError("", "Có lỗi xảy ra khi đăng ký. Vui lòng thử lại sau.");
                return View(model);
            }

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }

}
