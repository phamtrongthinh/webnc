using btlWEBNC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace btlWEBNC.Controllers
{
    public class AccountController : Controller
    {
        private readonly QuanLyLopHocTrucTuyen2Context _context;
        public AccountController(QuanLyLopHocTrucTuyen2Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            // Nếu TempData có thông báo thành công, gửi nó sang View để hiển thị
            ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
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
        /*
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

         */
        [HttpGet]
        public IActionResult Login()
        {
            // Nếu TempData có thông báo thành công, gửi nó sang View để hiển thị
            ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            return View();
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                // Kiểm tra model hợp lệ
                if (!ModelState.IsValid)
                {
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

                // Tạo Claims
                var claims = new List<Claim>
                 {
                     new Claim(ClaimTypes.Name, user.Username),
                     new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("UserID",user.UserId.ToString())
                   
               };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Thực hiện đăng nhập
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                      new ClaimsPrincipal(claimsIdentity));

                // Chuyển hướng theo role
                if (user.Role == "Student") return RedirectToAction("StudentIndex", "Student");
                if (user.Role == "Teacher") return RedirectToAction("Index", "Home");
                if (user.Role == "Admin") return RedirectToAction("Index", "Admin");

                return View();
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "Có lỗi xảy ra khi đăng nhập. Vui lòng thử lại sau.");
                return View();
            }

        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login"); // Chuyển về trang login nếu chưa đăng nhập
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Lấy email từ session/claims của user đã đăng nhập
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
           
            if (string.IsNullOrEmpty(userEmail))
            {
                ModelState.AddModelError(string.Empty, "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.");
                return RedirectToAction("Login");
            }

            var user = await _context.TblUsers.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy tài khoản.");
                return View(model);
            }

            // Kiểm tra mật khẩu hiện tại có đúng không
            if (user.Password != model.CurrentPassword)
            {
                ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không đúng.");
                return View(model);
            }

            // Cập nhật mật khẩu mới
            user.Password = model.NewPassword;
            _context.TblUsers.Update(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công!";
            return View(model);
            
            
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        // Xử lý reset mật khẩu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.TblUsers.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {                
                ModelState.AddModelError(string.Empty, "Nếu email tồn tại trong hệ thống, mật khẩu sẽ được thay đổi.");
                return View(model);
            }

            // Cập nhật mật khẩu mới trong database
            user.Password = model.NewPassword;
            _context.TblUsers.Update(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công!";
            // Chuyển hướng đến trang Login với thông báo
            return RedirectToAction("Login");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Clear any session data
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }



    }
}
