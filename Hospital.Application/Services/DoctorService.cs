using AutoMapper;
using Hospital.Application.DTOs.DoctorDTOs;
using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Application.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        // Şifreleme işlemleri için AuthService'i kullanacağız
        private readonly IAuthService _authService; 

        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            // Branş bilgileriyle birlikte (Include) getiren özel repo metodunu kullanıyoruz.
            var doctors = await _unitOfWork.Doctors.GetAllDoctorsWithSpecialtyAsync();
            return _mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<List<DoctorDto>> GetBySpecialtyIdAsync(int specialtyId)
        {
             // Önce branş var mı kontrol et
             var specialty = await _unitOfWork.Specialties.GetByIdAsync(specialtyId);
             if(specialty == null) throw new Exception("Seçilen branş bulunamadı.");

             var doctors = await _unitOfWork.Doctors.GetDoctorsBySpecialtyIdAsync(specialtyId);
             return _mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetDoctorByIdWithSpecialtyAsync(id);
            if (doctor == null) throw new Exception("Doktor bulunamadı.");
            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task CreateAsync(CreateDoctorDto createDto)
        {
            if (await _unitOfWork.Users.GetByEmailAsync(createDto.Email) != null)
            {
                throw new Exception("Bu e-posta adresi zaten kullanımda.");
            }

            if (await _unitOfWork.Specialties.GetByIdAsync(createDto.SpecialtyId) == null)
            {
                throw new Exception("Seçilen uzmanlık alanı bulunamadı.");
            }

            _authService.CreatePasswordHash(createDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

          
            var doctor = _mapper.Map<Doctor>(createDto);

            doctor.PasswordHash = passwordHash;
            doctor.PasswordSalt = passwordSalt;

            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(UpdateDoctorDto updateDto)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(updateDto.Id);
            if (doctor == null) throw new Exception("Güncellenecek doktor bulunamadı.");

            if (doctor.SpecialtyId != updateDto.SpecialtyId)
            {
                 if (await _unitOfWork.Specialties.GetByIdAsync(updateDto.SpecialtyId) == null)
                 {
                     throw new Exception("Yeni seçilen uzmanlık alanı bulunamadı.");
                 }
            }

           
            _mapper.Map(updateDto, doctor);
            
            _unitOfWork.Doctors.Update(doctor);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null) throw new Exception("Silinecek doktor bulunamadı.");

            _unitOfWork.Doctors.Remove(doctor); 
            await _unitOfWork.CompleteAsync();
        }
    }
}