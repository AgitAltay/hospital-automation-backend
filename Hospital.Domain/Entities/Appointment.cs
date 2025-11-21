using System;

namespace Hospital.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public int DoctorId { get; set; } 
        public Doctor? Doctor { get; set; } 

        public int PatientId { get; set; } 
        public User? Patient { get; set; } 

        public DateTime AppointmentDate { get; set; }
        public string? Status { get; set; } 
        public string? Notes { get; set; }
    }
}