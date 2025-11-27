using Hospital.Domain.Entities;

namespace Hospital.Domain.Interfaces;

public interface IAppointmentRepository
{
    public Task<List<Appointment>> GetAllByPatientIdAsync(string patientId);
    public Task<List<Appointment>> GetAllByPatientIdAndDateAsync(string patientId, DateTime date);
    public Task<List<Appointment>> GetByAppointmentIdAsync(string appointmentId);
    
}