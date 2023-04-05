using System.Data;
using System.Diagnostics;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace StudentsWebApp.Controllers
{
    public class SignupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Index(User newUser)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (UserProvider.HasAccount(newUser.Name))
                ModelState.AddModelError("name", $"Користувач {newUser.Name} вжє зареєстрований. Спробуйте інше ім'я");
            else if (UserProvider.TryAddUser(newUser))
            {
                Trace.WriteLine($"{DateTime.Now:HH:mm:ss}: \"{newUser.Name}\" is registered");
                HttpContext.Session.SetString("Username", newUser.Name);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("name", $"Збій системи. Спробуйте зареєструватися пізніше");
            }

            return View();
        }
    }
}