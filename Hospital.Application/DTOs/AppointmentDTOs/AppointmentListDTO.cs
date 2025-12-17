using Hospital.Domain.Enums;

namespace Hospital.Application.DTOs.AppointmentDTOs
{
    public class AppointmentListDto
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
        
        
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        
        public string DepartmentName { get; set; } = string.Empty; 
    }
}