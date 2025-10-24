using System.ComponentModel.DataAnnotations;

namespace MvcTask01.Models
{
    public class BookingViewModel
    {
        [Required(ErrorMessage = "Patient name is required")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Appointment date is required")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Doctor ID is required")]
        public int DoctorId { get; set; }

        public string AppointmentTime { get; set; }
    }
}
