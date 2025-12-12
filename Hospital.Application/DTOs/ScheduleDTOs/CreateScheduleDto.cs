using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.DTOs.ScheduleDTOs
{
    public class CreateScheduleDto
    {
        [Required]
        public DayOfWeek DayOfWeek { get; set; } 

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }
    }
}