using Hospital.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.DTOs.DoctorDTOs
{
    public class UpdateDoctorDto
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }
        
        [Required]
        public Gender Gender { get; set; }

        [Required, MaxLength(50)]
        public string LicenseNumber { get; set; }

        [Required]
        public int SpecialtyId { get; set; }
    }
}