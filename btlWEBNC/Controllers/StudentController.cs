using Microsoft.AspNetCore.Mvc;

namespace btlWEBNC.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
