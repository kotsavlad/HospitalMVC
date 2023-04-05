using Hospital.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class PatientsController : Controller
    {
        // GET: PatientsController
        public ActionResult Index()
        {
            var patients = DataProvider.Patients;
            return View(patients);
        }
    }
}
