using Hospital.Application.DTOs.AppointmentDTOs;
using Hospital.Domain.Enums;

namespace Hospital.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task CreateAsync(CreateAppointmentDto createDto);
        Task UpdateAsync(UpdateAppointmentDto updateDto);
        Task CreatePublicAsync(CreateAppointmentPublicDto input);
        Task CancelPublicAsync(CancelAppointmentPublicDto input);
        Task<List<AppointmentListDto>> SearchPublicAsync(ValidatePatientDto input);

        Task CancelAppointmentAsync(int appointmentId, string cancellationReason);
        Task<AppointmentListDto> GetByIdAsync(int appointmentId);
        Task<List<AppointmentListDto>> GetAllByPatientIdAsync(int patientId);
        Task<List<AppointmentListDto>> GetAllByDoctorIdAsync(int doctorId, DateTime date);
        Task<List<AppointmentListDto>> SearchAppointmentsAsync(int? doctorId, int? patientId, DateTime? startDate, DateTime? endDate);
    }
}