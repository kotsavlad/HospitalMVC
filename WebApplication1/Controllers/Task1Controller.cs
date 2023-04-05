using Hospital.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Hospital.Controllers;

public class Task1Controller : Controller
{
    // GET: TestController
    private static List<SelectListItem>? _allYears;

    public ActionResult Index(int? year = null)
    {
        var pairs = DataProvider.MaxAges(year);
        if (_allYears is null)
        {
            _allYears = DataProvider.Visits
                .Where(v => v.Date is not null)
                .Select(v => v.Date?.Year)
                .Distinct()
                .OrderBy(y => y)
                .Select(y => new SelectListItem { Value = y.ToString(), Text = y.ToString() }).ToList();
            _allYears.Insert(0, new SelectListItem { Value = "Усі роки", Text = "Усі роки" });
        }

        Triplet triplet = new()
        {
            Pairs = pairs,
            Years = _allYears,
            YearString = year?.ToString()
        };
        return View(triplet);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Index(Triplet triplet)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("State is not valid");
            return RedirectToAction("Index");
        }

        int? year = null;
        if (int.TryParse(triplet.YearString, out var yearValue))
            year = yearValue;
        return Index(year);
    }

    // GET: TestController/Edit/5
}