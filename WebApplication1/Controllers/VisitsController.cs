using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    public class VisitsController : Controller
    {
        private IEnumerable<NamedVisit>? _namedVisits;
        public IActionResult Index()
        {
            if (_namedVisits is null)
            {
                var doctors = DataProvider.DoctorsDictionary;
                var patients = DataProvider.PatientsDictionary;
                var visits = DataProvider.Visits;
                _namedVisits = visits.Select(v => new 
                    NamedVisit(doctors[v.DoctorId].Name, patients[v.PatientId].Name, v.Date));
            }
            return View(_namedVisits);
        }
    }
}
