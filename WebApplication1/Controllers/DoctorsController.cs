using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class DoctorsController : Controller
    {
        private IEnumerable<Doctor>? _doctors;
        public IActionResult Index()
        {
            _doctors ??= DataProvider.Doctors;
            return View(_doctors);
        }
    }
}
