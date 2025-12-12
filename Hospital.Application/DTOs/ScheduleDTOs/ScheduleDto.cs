using System;

namespace Hospital.Application.DTOs.ScheduleDTOs
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public string DayOfWeek { get; set; } 
        public string StartTime { get; set; } 
        public string EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}