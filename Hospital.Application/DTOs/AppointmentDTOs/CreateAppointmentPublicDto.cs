using System.ComponentModel.DataAnnotations;
using Hospital.Application.Attributes; 


namespace Hospital.Application.DTOs.AppointmentDTOs
{
    public class CreateAppointmentPublicDto
    {
        
        [Required(ErrorMessage = "T.C. Kimlik Numarası zorunludur.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "T.C. Kimlik Numarası 11 haneli olmalıdır.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "T.C. Kimlik Numarası sadece rakamlardan oluşmalıdır.")]
        public string TcKimlikNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } 

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string? Email { get; set; }


        [Required(ErrorMessage = "Doktor seçimi zorunludur.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Randevu tarihi zorunludur.")]
        [FutureDate(ErrorMessage = "Geçmiş bir tarihe randevu alamazsınız.")]
        
        public DateTime AppointmentDate { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }
    }
}