using Hospital.Domain.Entities;
using Hospital.Domain.Enums;

namespace Hospital.Domain.Interfaces;

public interface IAppointmentRepository :  IGenericRepository<Appointment>
{
    public Task<List<Appointment>> GetAllByPatientIdAsync(int patientId);
    public Task<List<Appointment>> GetAllByPatientIdAndDateAsync(int patientId, DateTime date);
    public Task<Appointment?> GetByAppointmentIdAsync(int appointmentId);
    Task<List<Appointment>> GetPatientHistoryAsync(int patientId);
    Task<List<Appointment>> GetPatientUpcomingAppointmentsAsync(int patientId);
    Task<List<Appointment>> GetDoctorDailyScheduleAsync(int doctorId, DateTime date);
    Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime date);
    Task<List<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status);
    Task<bool> IsPatientAvailableAsync(int patientId, DateTime date);
    Task<int> GetPatientActiveAppointmentCountAsync(int patientId);
    Task<List<Appointment>> GetByFilterAsync(int? doctorId, int? patientId, DateTime? startDate, DateTime? endDate);
    Task<List<Appointment>> GetDailyAppointmentsByDepartmentAsync(int departmentId, DateTime date);
    
}