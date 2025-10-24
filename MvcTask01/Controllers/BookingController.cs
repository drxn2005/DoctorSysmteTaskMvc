using Microsoft.AspNetCore.Mvc;
using MvcTask01.DataAccess;
using MvcTask01.Models;
using System.Globalization;

public class BookingController : Controller
{
    private readonly ApplicationDbContext _context;

    public BookingController(ApplicationDbContext context)
    {
        _context = context;
    }

    // API endpoint to get booked times for a specific date
    [HttpGet]
    public IActionResult GetBookedTimes(DateTime date)
    {
        var dateStart = date.Date;
        var dateEnd = dateStart.AddDays(1);

        var bookedTimes = _context.Bookings
                               .Where(b => b.AppointmentDate >= dateStart && b.AppointmentDate < dateEnd)
                               .Select(b => b.AppointmentDate.ToString("h:mm tt"))
                               .ToList();

        return Json(new { bookedTimes });
    }

    [HttpPost]
    public IActionResult SubmitBooking(Booking booking, string appointmentTime)
    {
        // تجاهل أخطاء الـ Doctor property لأنها مش جزء من الـ Form
        ModelState.Remove("Doctor");

        if (ModelState.IsValid)
        {
            DateTime appointmentDateTime = DateTime.ParseExact(
                $"{booking.AppointmentDate.ToShortDateString()} {appointmentTime}",
                "M/d/yyyy h:mm tt",
                CultureInfo.InvariantCulture
            );

            // Check if the appointment is in the past
            if (appointmentDateTime <= DateTime.Now)
            {
                TempData["Error"] = "Cannot book appointments in the past or past times on the same day.";
                return RedirectToAction("Book", "Home", new { doctorId = booking.DoctorId });
            }

            // Check if it's Friday or Saturday
            if (appointmentDateTime.DayOfWeek == DayOfWeek.Friday ||
                appointmentDateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                TempData["Error"] = "Fridays and Saturdays are not available for appointments.";
                return RedirectToAction("Book", "Home", new { doctorId = booking.DoctorId });
            }

            booking.AppointmentDate = appointmentDateTime;

            // Check for existing booking
            var existingBooking = _context.Bookings
                                         .FirstOrDefault(b => b.AppointmentDate == booking.AppointmentDate &&
                                                             b.DoctorId == booking.DoctorId);

            if (existingBooking != null)
            {
                TempData["Error"] = "This appointment time is already booked. Please choose a different time.";
                return RedirectToAction("Book", "Home", new { doctorId = booking.DoctorId });
            }

            var doctor = _context.Doctors.FirstOrDefault(d => d.Id == booking.DoctorId);
            if (doctor == null)
            {
                TempData["Error"] = "Invalid Doctor ID.";
                return RedirectToAction("Index", "Home");
            }

            // إضافة الحجز إلى الداتابيز
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            TempData["SuccessMessage"] = $"Your appointment with Dr. {doctor.Name} has been successfully booked!";
            return RedirectToAction("BookingConfirmation");
        }

        // Debugging: شوف أي errors باقية
        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        TempData["Error"] = $"Validation errors: {string.Join(", ", errors)}";
        return RedirectToAction("Book", "Home", new { doctorId = booking.DoctorId });
    }
    private List<string> GetAvailableTimes()
    {
        List<string> availableTimes = new List<string>();

        DateTime startTime = DateTime.Today.AddHours(9); // 9 AM
        DateTime endTime = DateTime.Today.AddHours(17); // 5 PM

        for (var time = startTime; time < endTime; time = time.AddMinutes(30))
        {
            availableTimes.Add(time.ToString("h:mm tt"));
        }

        return availableTimes;
    }

    public IActionResult BookingConfirmation()
    {
        return View();
    }
}