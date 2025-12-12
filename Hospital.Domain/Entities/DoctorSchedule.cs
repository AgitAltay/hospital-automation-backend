using System;

namespace Hospital.Domain.Entities
{
    public class DoctorSchedule : BaseEntity
    {
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        
        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}