using AutoMapper;
using Hospital.Application.DTOs.AppointmentDTOs;
using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;
using Hospital.Domain.Interfaces; // IUnitOfWork burada

namespace Hospital.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateAppointmentDto createDto)
        {
            var isDoctorAvailable = await _unitOfWork.Appointments
                .IsDoctorAvailableAsync(createDto.DoctorId, createDto.AppointmentDate);

            if (!isDoctorAvailable)
                throw new Exception("Seçilen saatte doktor müsait değil veya başka bir randevusu var.");

            var isPatientAvailable = await _unitOfWork.Appointments
                .IsPatientAvailableAsync(createDto.PatientId, createDto.AppointmentDate);

            if (!isPatientAvailable)
                throw new Exception("Hastanın bu saatte başka bir randevusu zaten var.");

            var appointment = _mapper.Map<Appointment>(createDto);
            
            appointment.Status = AppointmentStatus.Active; 

            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.CompleteAsync(); 
        }

        // --- 2. RANDEVU İPTALİ ---
        public async Task CancelAppointmentAsync(int appointmentId, string cancellationReason)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);
            
            if (appointment == null) 
                throw new Exception("Randevu bulunamadı.");

            appointment.Status = AppointmentStatus.Cancelled;

            _unitOfWork.Appointments.Update(appointment);
            await _unitOfWork.CompleteAsync();
        }

        
        public async Task<List<AppointmentListDto>> GetAllByPatientIdAsync(int patientId)
        {
            var appointments = await _unitOfWork.Appointments.GetAllByPatientIdAsync(patientId);
            
            return _mapper.Map<List<AppointmentListDto>>(appointments);
        }

        public async Task<List<AppointmentListDto>> GetAllByDoctorIdAsync(int doctorId, DateTime date)
        {
            var appointments = await _unitOfWork.Appointments.GetDoctorDailyScheduleAsync(doctorId, date);
            return _mapper.Map<List<AppointmentListDto>>(appointments);
        }

        public async Task<AppointmentListDto> GetByIdAsync(int appointmentId)
        {
             var appointment = await _unitOfWork.Appointments.GetByAppointmentIdAsync(appointmentId);
             
             if (appointment == null) throw new Exception("Randevu bulunamadı");

             return _mapper.Map<AppointmentListDto>(appointment);
        }

        public async Task UpdateAsync(UpdateAppointmentDto updateDto)
        {
             var appointment = await _unitOfWork.Appointments.GetByIdAsync(updateDto.AppointmentId);
             if (appointment == null) throw new Exception("Randevu bulunamadı");

             if(updateDto.NewDate.HasValue && updateDto.NewDate != appointment.AppointmentDate)
             {
                 bool isAvailable = await _unitOfWork.Appointments
                     .IsDoctorAvailableAsync(appointment.DoctorId, updateDto.NewDate.Value);
                 
                 if(!isAvailable) throw new Exception("Yeni tarih için doktor müsait değil.");
                 
                 appointment.AppointmentDate = updateDto.NewDate.Value;
             }
             
             appointment.Status = updateDto.Status;
             if(!string.IsNullOrEmpty(updateDto.Note)) appointment.Notes = updateDto.Note;

             _unitOfWork.Appointments.Update(appointment);
             await _unitOfWork.CompleteAsync();
        }

        public async Task<List<AppointmentListDto>> SearchAppointmentsAsync(int? doctorId, int? patientId, DateTime? startDate, DateTime? endDate)
        {
            var result = await _unitOfWork.Appointments.GetByFilterAsync(doctorId, patientId, startDate, endDate);
            return _mapper.Map<List<AppointmentListDto>>(result);
        }
    }
}