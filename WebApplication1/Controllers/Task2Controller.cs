using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class Task2Controller : Controller
    {
        public IActionResult Index()
        {
            var list = DataProvider.VisitAllDoctors();
            return View(list);
        }
    }
}
