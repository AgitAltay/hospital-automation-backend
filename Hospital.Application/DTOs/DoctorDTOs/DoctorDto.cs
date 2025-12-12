namespace Hospital.Application.DTOs.DoctorDTOs
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; } 

        public string LicenseNumber { get; set; }
        public int SpecialtyId { get; set; }
        public string SpecialtyName { get; set; } 
    }
}