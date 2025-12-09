using Hospital.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.DTOs.DoctorDTOs
{
    public class CreateDoctorDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; } 

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Lisans numarası zorunludur.")]
        [MaxLength(50)]
        public string LicenseNumber { get; set; }

        [Required(ErrorMessage = "Uzmanlık alanı seçimi zorunludur.")]
        public int SpecialtyId { get; set; }
    }
}