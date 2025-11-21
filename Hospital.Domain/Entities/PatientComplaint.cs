namespace Hospital.Domain.Entities
{
    public class PatientComplaint : BaseEntity
    {
        public int PatientId { get; set; } 
        public User? Patient { get; set; } 

        public string? Subject { get; set; }
        public required string Description { get; set; }
        public DateTime ComplaintDate { get; set; }
        public string? Status { get; set; } 
    }
}