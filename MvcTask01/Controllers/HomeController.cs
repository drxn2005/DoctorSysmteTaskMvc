using Microsoft.AspNetCore.Mvc;
using MvcTask01.DataAccess;
using MvcTask01.Models;
using System.Diagnostics;

namespace MvcTask01.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var doctors = _db.Doctors.AsQueryable();
            return View(doctors.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Book(int doctorId)
        {
            var doctor = _db.Doctors.FirstOrDefault(d => d.Id == doctorId);
            if (doctor == null)
            {
                return NotFound();
            }

            var booking = new Booking
            {
                DoctorId = doctorId
            };

            ViewBag.Doctor = doctor;
            return View(booking);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}