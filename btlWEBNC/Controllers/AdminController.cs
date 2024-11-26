using btlWEBNC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace btlWEBNC.Controllers
{
    public class AdminController : Controller
    {
        private readonly QuanLyLopHocTrucTuyen2Context _context;

        public AdminController( QuanLyLopHocTrucTuyen2Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> TeacherManagement()
        {
            // Lọc danh sách chỉ lấy các user có Role là "student"
            {
                var students = await _context.TblUsers.Where(u => u.Role == "Teacher").ToListAsync();
                return View(students);
            }
        }

       
        public async Task<IActionResult> StudentManagement()
        // Lọc danh sách chỉ lấy các user có Role là "student"
        {
            var students = await _context.TblUsers.Where(u => u.Role == "Student").ToListAsync();
            return View(students);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.TblUsers.FindAsync(id);
            if (user != null)
            {
                _context.TblUsers.Remove(user);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int UserId, string Username, string Password)
        {
            var user = await _context.TblUsers.FindAsync(UserId);
            if (user != null)
            {
                user.Username = Username;
                user.Password = Password;

                try
                {
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch
                {
                    return Json(new { success = false });
                }
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> PhanQuyen()
        {
            // Retrieve the list of all users from the database
            var users = await _context.TblUsers.ToListAsync();
            return View("PhanQuyen",users);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateRole(int UserId, string Role)
        {
            var user = await _context.TblUsers.FindAsync(UserId);
            if (user != null)
            {
                user.Role = Role;

                try
                {
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch
                {
                    return Json(new { success = false });
                }
            }
            return Json(new { success = false });
        }




       



    }
}
