using btlWEBNC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IActionResult Index()
        {
            return View();
        }



        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public IActionResult CreateCourse()
        {
            return View();
        }

        // Handle the form submission
        [HttpPost]
        public async Task<IActionResult> CreateCourse(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {

                // Get the teacher's ID from the logged-in user
                // Retrieve the TeacherId as a string and attempt to parse it to an int
                var teacherIdString = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                int? teacherId = null;
                if (!string.IsNullOrEmpty(teacherIdString) && int.TryParse(teacherIdString, out int parsedTeacherId))
                {
                    teacherId = parsedTeacherId;
                }

                // Create a new Course entity to save to the database
                var newCourse = new TblCourse
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    TeacherId = teacherId // Use the retrieved Teacher ID
                };
                // Add to the database context and save changes
                _context.TblCourses.Add(newCourse);
                await _context.SaveChangesAsync();
                // Redirect to the index or another page after successful creation
                return RedirectToAction("Index");

            }
            return View(model);

        }



        public IActionResult Student()
        {
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }


    }
}
