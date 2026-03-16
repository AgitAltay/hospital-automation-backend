namespace Hospital.Application.DTOs.AppointmentDTOs
{
    public class CreateAIFeedbackDto
    {
        public int AppointmentId { get; set; }
        public required string PatientComplaint { get; set; }
        public bool IsCorrect { get; set; }
        public required string PredictedSpecialty { get; set; }
        public string? CorrectSpecialty { get; set; } 
    }
}
