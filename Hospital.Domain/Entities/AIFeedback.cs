using System;

namespace Hospital.Domain.Entities
{
    public class AIFeedback : BaseEntity
    {
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public required string PatientComplaint { get; set; }
        
        public bool IsCorrect { get; set; }
        
        public required string PredictedSpecialty { get; set; }
        
        public string? CorrectSpecialty { get; set; } 
    }
}
