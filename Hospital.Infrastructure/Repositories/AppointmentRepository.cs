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

    public async Task<Appointment?> GetByAppointmentIdAsync(int id)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.Id == id); 
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
    
    
    // hastanın aynı saate randevu almasını engellemek için
    public async Task<bool> IsPatientAvailableAsync(int patientId, DateTime date)
    {
        bool isBusy = await _context.Appointments
            .AnyAsync(a => a.PatientId == patientId 
                           && a.AppointmentDate == date 
                           && a.Status != AppointmentStatus.Cancelled);

        return !isBusy;
    }
    
    // hastanın aktif randevularını sayı olarak döndürür. Daha sonra alınabilecek randevu sayısını kısıtlamak için
    public async Task<int> GetPatientActiveAppointmentCountAsync(int patientId)
    {
        return await _context.Appointments
            .CountAsync(a => a.PatientId == patientId 
                             && a.Status == AppointmentStatus.Active);
    }

    // genel filtreleme 
    public async Task<List<Appointment>> GetByFilterAsync(int? doctorId, int? patientId, DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Appointments.AsQueryable();

        query = query.Include(a => a.Patient).Include(a => a.Doctor).ThenInclude(d => d.Specialty);

        if (doctorId.HasValue)
            query = query.Where(a => a.DoctorId == doctorId.Value);

        if (patientId.HasValue)
            query = query.Where(a => a.PatientId == patientId.Value);

        if (startDate.HasValue)
            query = query.Where(a => a.AppointmentDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(a => a.AppointmentDate <= endDate.Value);

        return await query.OrderBy(a => a.AppointmentDate).ToListAsync();    }

    // departmanın günlük randevularını getirmek için
    public async Task<List<Appointment>> GetDailyAppointmentsByDepartmentAsync(int departmentId, DateTime date)
    {
        return await _context.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .Where(a => a.Doctor != null && a.Doctor.SpecialtyId == departmentId
             && a.AppointmentDate.Date == date.Date)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();
    }
}