using Hospital.Domain.Enums;

namespace Hospital.Domain.Entities
{
    public class Doctor : User 
    {
        public int SpecialtyId { get; set; } 
        public Specialty? Specialty { get; set; } 
        public string? LicenseNumber { get; set; }
    }
}