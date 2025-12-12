using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Infrastructure.Repositories
{
    public class DoctorScheduleRepository : GenericRepository<DoctorSchedule>, IDoctorScheduleRepository
    {
        public DoctorScheduleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<DoctorSchedule>> GetSchedulesByDoctorIdAsync(int doctorId)
        {
            return await _context.DoctorSchedules
                .Where(x => x.DoctorId == doctorId && !x.IsDeleted)
                .OrderBy(x => x.DayOfWeek) 
                .ToListAsync();
        }

        public async Task<DoctorSchedule?> GetScheduleByDoctorAndDayAsync(int doctorId, DayOfWeek day)
        {
            return await _context.DoctorSchedules
                .FirstOrDefaultAsync(x => x.DoctorId == doctorId && x.DayOfWeek == day && !x.IsDeleted);
        }
    }
}