using System.Collections.Generic;

namespace Hospital.Domain.Entities
{
    public class Specialty : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

       
        public ICollection<Doctor> Doctors { get; set; }

        public Specialty()
        {
            Doctors = new HashSet<Doctor>();
        }
    }
}