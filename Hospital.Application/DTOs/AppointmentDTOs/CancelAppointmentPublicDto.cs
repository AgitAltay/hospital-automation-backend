using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.DTOs.AppointmentDTOs
{
    public class CancelAppointmentPublicDto
    {
        [Required(ErrorMessage = "Randevu ID zorunludur.")]
        public int AppointmentId { get; set; }


        [Required(ErrorMessage = "T.C. Kimlik Numarası zorunludur.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "11 haneli TCKN giriniz.")]
        public string TcKimlikNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? CancellationReason { get; set; }
    }
}