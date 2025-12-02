using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.DTOs.SpecialtyDTOs
{
    public class UpdateSpecialtyDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Uzmanlık alanı adı zorunludur.")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}