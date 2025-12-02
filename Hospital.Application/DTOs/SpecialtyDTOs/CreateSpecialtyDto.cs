using System.ComponentModel.DataAnnotations; 

namespace Hospital.Application.DTOs.SpecialtyDTOs
{
    public class CreateSpecialtyDto
    {
        [Required(ErrorMessage = "Uzmanlık alanı adı zorunludur.")]
        [MaxLength(100, ErrorMessage = "Uzmanlık alanı adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }
    }
}