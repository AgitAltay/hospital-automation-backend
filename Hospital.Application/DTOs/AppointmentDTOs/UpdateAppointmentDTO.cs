using Hospital.Domain.Enums; // Enum'ı burada kullanacağız

namespace Hospital.Application.DTOs.AppointmentDTOs
{
    public class UpdateAppointmentDto
    {
        public int AppointmentId { get; set; } 
        public DateTime? NewDate { get; set; } 
        public AppointmentStatus Status { get; set; } 
        public string? Note { get; set; } 
    }
}