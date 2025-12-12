using Hospital.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Domain.Interfaces
{
    public interface IDoctorScheduleRepository : IGenericRepository<DoctorSchedule>
    {
        Task<List<DoctorSchedule>> GetSchedulesByDoctorIdAsync(int doctorId);
        
        Task<DoctorSchedule?> GetScheduleByDoctorAndDayAsync(int doctorId, DayOfWeek day);
    }
}