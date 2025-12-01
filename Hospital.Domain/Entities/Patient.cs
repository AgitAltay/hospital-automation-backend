using System;

namespace Hospital.Domain.Entities
{

    public class Patient : User
    {
        public DateTime? DateOfBirth { get; set; }

    }
}