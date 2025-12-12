namespace Hospital.Domain.Entities
{
    // User sınıfından miras alıyoruz.
    public class Doctor : User
    {
        public string LicenseNumber { get; set; } // Diploma/Lisans No
        
        public int SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }
    }
}