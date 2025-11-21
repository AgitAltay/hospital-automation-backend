using System.Collections.Generic;

namespace Hospital.Domain.Entities
{
    public class Role : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<User>? Users { get; set; } 
    }
}