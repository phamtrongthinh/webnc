using btlWEBNC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace btlWEBNC.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuanLyLopHocTrucTuyen2Context _context;

        public HomeController(QuanLyLopHocTrucTuyen2Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get the TeacherId from the claims
            var teacherIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (teacherIdClaim == null || !int.TryParse(teacherIdClaim.Value, out int teacherId))
            {
                TempData["Error"] = "Không thể xác định giáo viên. Vui lòng đăng nhập lại.";
                return RedirectToAction("Index", "Home");
            }
            // Get the list of courses created by this teacher
            var courses = await _context.TblCourses
                .Where(c => c.TeacherId == teacherId)
                .Select(c => new CourseListViewModel
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    Price = c.Price
                })
                .ToListAsync();

            return View(courses);
        }


        
         [HttpGet]
        public IActionResult RegisterCourse()
        {
            // Kiểm tra nếu không phải Teacher thì redirect về trang thông báo
            if (!User.IsInRole("Student"))
            {
                TempData["Error"] = "Chức năng này chỉ dành cho học sinh!";
                return RedirectToAction("Index", "Home"); // Hoặc redirect đến trang thông báo riêng
            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            // Kiểm tra nếu không phải Teacher thì redirect về trang thông báo
            if (!User.IsInRole("Teacher"))
            {
                TempData["Error"] = "Chức năng này chỉ dành cho giáo viên!";
                return RedirectToAction("Index", "Home"); // Hoặc redirect đến trang thông báo riêng
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")] // Thêm authorize attribute
        public async Task<IActionResult> CreateCourse(CourseViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // Lấy TeacherId từ Claims
                var teacherIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
                if (teacherIdClaim == null || !int.TryParse(teacherIdClaim.Value, out int teacherId))
                {
                    ModelState.AddModelError("", "Không thể xác định giáo viên. Vui lòng đăng nhập lại.");
                    return View(model);
                }

                // Kiểm tra xem user có tồn tại và có phải là giáo viên không
                var teacher = await _context.TblUsers
                    .FirstOrDefaultAsync(u => u.UserId == teacherId && u.Role == "Teacher");
                if (teacher == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy thông tin giáo viên hoặc không có quyền tạo khóa học.");
                    return View(model);
                }

                var newCourse = new TblCourse
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    TeacherId = teacherId,
                    // Có thể thêm các trường khác như CreateDate = DateTime.Now
                };

                _context.TblCourses.Add(newCourse);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Tạo khóa học thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo khóa học. Vui lòng thử lại.");
                return View(model);
            }
        }



        


    }
}
