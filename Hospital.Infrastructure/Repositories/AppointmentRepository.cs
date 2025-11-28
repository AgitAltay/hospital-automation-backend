using Hospital.Domain.Entities;
using Hospital.Domain.Enums;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories;

public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Appointment>> GetAllByPatientIdAsync(int patientId)
    {
        return await _context.Appointments
            .Where(a => a.PatientId == patientId)
            .Include(a => a.Patient)
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetAllByPatientIdAndDateAsync(int patientId, DateTime date)
    {
        return await _context.Appointments
            .Where(a => a.PatientId == patientId && a.AppointmentDate == date)
            .Include(a => a.Patient)
            .ToListAsync();
    }

    public async Task<Appointment?> GetByAppointmentIdAsync(int appointmentId)
    {
        return await _context.Appointments
            .Include(a => a.Patient) 
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
    }

    public async Task<List<Appointment>> GetPatientHistoryAsync(int patientId)
    {
        return await _context.Appointments
            .Where(a => a.PatientId == patientId && a.AppointmentDate < DateTime.Now)
            .Include(a => a.Doctor)
            .Include(a => a.Doctor.Specialty)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetPatientUpcomingAppointmentsAsync(int patientId)
    {
        return await _context.Appointments
            .Where(a => a.PatientId == patientId && a.AppointmentDate >= DateTime.Now)
            .Include(a => a.Doctor)
            .Include(a => a.Doctor.Specialty)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetDoctorDailyScheduleAsync(int doctorId, DateTime date)
    {
        return await _context.Appointments
            .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == date.Date)
            .Include(a => a.Patient)
            .OrderBy(a => a.AppointmentDate) 
            .ToListAsync();
    }

    public async Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime date)
    {
        bool isBooked = await _context.Appointments
            .AnyAsync(a => a.DoctorId == doctorId 
                           && a.AppointmentDate == date
                           && a.Status != AppointmentStatus.Cancelled);

        return !isBooked;
    }

    public async Task<List<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status)
    {
        return await _context.Appointments
            .Where(a => a.Status == status)
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();
    }
}