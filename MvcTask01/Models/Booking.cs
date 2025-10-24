using ContactDoctor.Models;

namespace MvcTask01.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string PatientName { get; set; }  
        public int Age { get; set; }
        public string Phone { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
