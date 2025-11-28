using System;
using Hospital.Domain.Enums;

namespace Hospital.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public double AppointmentId { get; set; }
        public int DoctorId { get; set; } 
        public Doctor? Doctor { get; set; } 

        public int PatientId { get; set; } 
        public User? Patient { get; set; } 

        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}