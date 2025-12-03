using System.Reflection;

using Hospital.Domain.Enums;


namespace Hospital.Domain.Entities
{
    public class User : BaseEntity
    {
        public required string Email { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public int RoleId { get; set; } 
        public Role? Role { get; set; }
        public Gender Gender { get; set; }

    }
}