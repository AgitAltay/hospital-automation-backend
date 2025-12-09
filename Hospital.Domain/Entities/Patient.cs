using Hospital.Domain.Enums;

namespace Hospital.Domain.Entities
{
    public class Patient : BaseEntity
    {
        public required string TcKimlikNo { get; set; }

        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Email { get; set; } 

        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }

    }
}