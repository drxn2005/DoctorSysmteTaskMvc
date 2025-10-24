using System.Diagnostics;
using ContactDoctor.Models;
using Microsoft.AspNetCore.Mvc;
using MvcTask01.DataAccess;
using MvcTask01.Models;

namespace MvcTask01.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationDbContext _db = new ApplicationDbContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            return View(doctor); 
        }

        [HttpPost]
        public IActionResult SubmitBooking(Booking booking)
        {
            if (ModelState.IsValid)
            {
                // التحقق إذا كان الموعد محجوزًا بالفعل
                var existingBooking = _db.Bookings.FirstOrDefault(b => b.AppointmentDate == booking.AppointmentDate && b.DoctorId == booking.DoctorId);
                                         

                if (existingBooking != null)
                {
                    ModelState.AddModelError("", "This appointment time is already booked. Please choose a different time.");
                    // جلب الطبيب في حال حدوث خطأ
                    var doctorOnError = _db.Doctors.FirstOrDefault(d => d.Id == booking.DoctorId);
                    return View("Book", doctorOnError);
                }

                // جلب الطبيب بناءً على DoctorId
                var doctor = _db.Doctors.FirstOrDefault(d => d.Id == booking.DoctorId);

                if (doctor == null)
                {
                    ModelState.AddModelError("", "Invalid Doctor ID.");
                    return NotFound();
                }

                // ربط الـ Booking بالطبيب
                booking.Doctor = doctor;  // التأكد من ربط الطبيب بـ Booking

                // إضافة الحجز إلى قاعدة البيانات
                _db.Bookings.Add(booking);
                _db.SaveChanges();

                // تخزين رسالة النجاح في TempData
                TempData["SuccessMessage"] = $"Your appointment with Dr. {doctor.Name} has been successfully booked!";

                // إعادة التوجيه إلى صفحة التأكيد
                return RedirectToAction("BookingConfirmation");
            }

            var doctorForView = _db.Doctors.FirstOrDefault(d => d.Id == booking.DoctorId);

            return View("Book", doctorForView);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
