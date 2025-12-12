using Hospital.Application.DTOs.ScheduleDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Application.Interfaces
{
    public interface IDoctorScheduleService
    {
        Task<List<ScheduleDto>> GetSchedulesByDoctorIdAsync(int doctorId);

        Task<ScheduleDto> CreateAsync(CreateScheduleDto createDto, int doctorId);

        Task UpdateAsync(UpdateScheduleDto updateDto, int doctorId); 

        Task DeleteAsync(int id, int doctorId);
    }
}