using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories;

public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Appointment>> GetAllByPatientIdAsync(string patientId)
    {
        return await _dbSet.Include(a => a.PatientId).ToListAsync();
    }

    public async Task<List<Appointment>> GetAllByPatientIdAndDateAsync(string patientId, DateTime date)
    {
        return await _dbSet.Include(a => a.PatientId).Include(a=>a.AppointmentDate).ToListAsync();
    }

    public async Task<List<Appointment>> GetByAppointmentIdAsync(string appointmentId)
    {
        return await _dbSet.Include(a=>a.AppointmentId).ToListAsync();
    }
}