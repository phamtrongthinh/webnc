using btlWEBNC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace btlWEBNC.Controllers
{
    public class StudentController : Controller
    {
        private readonly QuanLyLopHocTrucTuyen2Context _context;

        public StudentController(QuanLyLopHocTrucTuyen2Context context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> StudentIndex()
        {
           
            // Get the current student's ID from the claims
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            if (!int.TryParse(userId, out int studentId))
            {
                TempData["Error"] = "Không thể xác định thông tin học sinh.";
                return RedirectToAction("StudentIndex", "Student");
            }

            // Retrieve the list of courses that the student has registered for
            var enrolledCourses = await _context.TblEnrollments
                .Where(e => e.StudentId == studentId)
                .Select(e => new EnrollmentListViewModel
                {
                    CourseId = e.Course.CourseId,
                    Title = e.Course.Title,
                    Description = e.Course.Description,
                    Price = e.Course.Price,
                    TeacherName = e.Course.Teacher != null ? e.Course.Teacher.Username : "N/A", // Assuming Teacher navigation property
                   
                })
                .ToListAsync();

            return View(enrolledCourses); // Pass the list to the Index view
        }

        public async Task<IActionResult> ViewCourseDetails(int courseId)
        {
            // Get the current student's ID from the claims
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            if (!int.TryParse(userId, out int studentId))
            {
                TempData["Error"] = "Không thể xác định thông tin học sinh.";
                return RedirectToAction("StudentIndex", "Student");
            }

            // Verify if the student is enrolled in the course
            var enrollment = await _context.TblEnrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enrollment == null)
            {
                TempData["Error"] = "Bạn không đăng ký khóa học này.";
                return RedirectToAction("StudentIndex", "Student");
            }

            // Retrieve course information along with learning materials
            var courseDetails = await _context.TblCourses
                .Include(c => c.TblLearningMaterials) // Assuming there's a navigation property for LearningMaterials
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (courseDetails == null)
            {
                TempData["Error"] = "Không tìm thấy khóa học.";
                return RedirectToAction("StudentIndex", "Student");
            }

            // Prepare a view model to pass course details and learning materials
            var courseDetailViewModel = new CourseDetailViewModel
            {
                CourseId = courseDetails.CourseId,
                Title = courseDetails.Title,
                Description = courseDetails.Description,
                Price = courseDetails.Price,
                TeacherName = courseDetails.Teacher != null ? courseDetails.Teacher.Username : "N/A",
                LearningMaterials = courseDetails.TblLearningMaterials.Select(lm => new LearningMaterialViewModel2
                {
                    MaterialId = lm.MaterialId,       // This now matches the view model property
                    Title = lm.Title,
                    Description = lm.Description,
                    CreatedDate = lm.CreatedDate      // This now matches the view model property
                }).ToList()
            };

            return View(courseDetailViewModel); // Create a view named ViewCourseDetails.cshtml
        }





        [HttpGet]
        public IActionResult StudentCreateCourse()
        {
            if (!User.IsInRole("Teacher"))
            {
                TempData["Error"] = "Chức năng này chỉ dành cho giáo viên";
                return RedirectToAction("StudentIndex", "Student");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> StudentRegisterCourse()
        {
           // Get list of courses that the student is not already enrolled in
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            if (!int.TryParse(userId, out int studentId))
            {
                TempData["Error"] = "Không thể xác định thông tin học sinh.";
                return RedirectToAction("StudentIndex", "Student");
            }

            var availableCourses = await _context.TblCourses
                .Where(c => !_context.TblEnrollments.Any(e => e.StudentId == studentId && e.CourseId == c.CourseId))
                .Select(c => new EnrollmentListViewModel
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    Price = c.Price,
                    TeacherName = c.Teacher != null ? c.Teacher.Username : "N/A" // Assuming Teacher navigation property
                })
                .ToListAsync();

            if (availableCourses == null || !availableCourses.Any())
            {
                TempData["Error"] = "Không có khóa học nào để hiển thị. lmao lamo";
                return RedirectToAction("StudentIndex", "Student");
            }

            return View(availableCourses); // View will be RegisterCourse.cshtml
          
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StudentRegisterCourse(int CourseID)        {

            // Get the current student's ID from the claims
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            if (!int.TryParse(userId, out int studentId))
            {
                TempData["Error"] = "Không thể xác định thông tin học sinh.";
                return RedirectToAction("StudentIndex", "Student");
            }

            // Check if the student is already enrolled in the course
            var existingEnrollment = await _context.TblEnrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == CourseID);

            if (existingEnrollment != null)
            {
                TempData["Error"] = "Bạn đã đăng ký khóa học này rồi.";
                return RedirectToAction("StudentRegisterCourse", "Student");
            }

            // Create a new enrollment
            var newEnrollment = new TblEnrollment
            {
                StudentId = studentId,
                CourseId = CourseID,
                EnrollmentDate = DateTime.Now
            };

            // Save to the database
            _context.TblEnrollments.Add(newEnrollment);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký khóa học thành công!";
            return RedirectToAction("StudentIndex", "Student");
        }







    }

}
