using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MvcTask01.Models;
using System.ComponentModel.DataAnnotations;

namespace MvcTask01.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public string PatientName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [ValidateNever] 
        public Doctor Doctor { get; set; }
    }
}