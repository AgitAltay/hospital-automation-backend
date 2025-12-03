using AutoMapper;
using Hospital.Application.DTOs.AppointmentDTOs;
using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;
using Hospital.Domain.Interfaces;
using Hospital.Application.Services;

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

        public async Task CreatePublicAsync(CreateAppointmentPublicDto input)
        {

            var isDoctorAvailable = await _unitOfWork.Appointments
                .IsDoctorAvailableAsync(input.DoctorId, input.AppointmentDate);

            if (!isDoctorAvailable)
            {
                throw new Exception("Seçilen tarih ve saatte doktor müsait değil.");
            }


            Patient patient;

            // DİKKAT: Burası çalışmayacak çünkü IUnitOfWork içinde henüz
            // 'Patients' adında özel bir repository ve 'GetByTcKimlikNoAsync' metodu yok.
            // Bir sonraki adımda Infrastructure katmanında bunu ekleyeceğiz.
            // Şimdilik mantığı kuralım.
            /*
            patient = await _unitOfWork.Patients.GetByTcKimlikNoAsync(input.TcKimlikNo);

            if (patient == null)
            {
                // Hasta yoksa, formdan gelen bilgilerle yeni bir hasta entity'si oluştur.
                // AutoMapper burada devreye giriyor.
                patient = _mapper.Map<Patient>(input);

                // Yeni hastayı veritabanına ekle.
                await _unitOfWork.Patients.AddAsync(patient);
                
                // Değişiklikleri kaydet ki hastanın yeni ID'si oluşsun.
                await _unitOfWork.CompleteAsync();
            }
            */
            // GEÇİCİ KOD (Altyapı hazır olana kadar derleme hatası vermemesi için):
            throw new NotImplementedException("Altyapı katmanında (Repository) TCKN ile hasta arama metodu henüz hazırlanmadı.");


            // 3. Randevu Oluşturma
            // DTO'dan Appointment entity'sine dönüşüm yap.
            var appointment = _mapper.Map<Appointment>(input);

            // Bulunan veya yeni oluşturulan hastanın ID'sini ata.
            // appointment.PatientId = patient.Id; // Altyapı hazır olunca açılacak

            // Randevuyu kaydet
            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<AppointmentListDto>> SearchAppointmentsAsync(int? doctorId, int? patientId, DateTime? startDate, DateTime? endDate)
        {
            var result = await _unitOfWork.Appointments.GetByFilterAsync(doctorId, patientId, startDate, endDate);
            return _mapper.Map<List<AppointmentListDto>>(result);
        }
    }
}