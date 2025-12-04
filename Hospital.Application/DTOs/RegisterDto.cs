using Hospital.Domain.Enums; // RoleType enum'ı için gerekli
using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        
        [Required]
        public RoleType Role { get; set; }

        public Gender Gender { get; set; }
    }
}