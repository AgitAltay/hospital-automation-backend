using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.DTOs.ScheduleDTOs
{
    public class UpdateScheduleDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}