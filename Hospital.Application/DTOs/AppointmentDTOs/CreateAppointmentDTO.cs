namespace Hospital.Application.DTOs.AppointmentDTOs
{
    public class CreateAppointmentDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        
        public string? Note { get; set; } 
    }
}