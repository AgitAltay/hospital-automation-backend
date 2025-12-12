using AutoMapper;
using Hospital.Application.DTOs.ScheduleDTOs;
using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Application.Services.Implementations
{
    public class DoctorScheduleService : IDoctorScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DoctorScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ScheduleDto>> GetSchedulesByDoctorIdAsync(int doctorId)
        {
            var schedules = await _unitOfWork.DoctorSchedules.GetSchedulesByDoctorIdAsync(doctorId);
            return _mapper.Map<List<ScheduleDto>>(schedules);
        }

        public async Task<ScheduleDto> CreateAsync(CreateScheduleDto createDto, int doctorId)
        {
            var existingSchedule = await _unitOfWork.DoctorSchedules.GetScheduleByDoctorAndDayAsync(doctorId, createDto.DayOfWeek);
            
            if (existingSchedule != null)
            {
                throw new InvalidOperationException($"'{createDto.DayOfWeek}' günü için zaten bir çalışma planınız var. Lütfen mevcut planı güncelleyin.");
            }

            if (createDto.StartTime >= createDto.EndTime)
            {
                throw new ArgumentException("Başlangıç saati, bitiş saatinden önce olmalıdır.");
            }

            var schedule = _mapper.Map<DoctorSchedule>(createDto);
            schedule.DoctorId = doctorId; 

            await _unitOfWork.DoctorSchedules.AddAsync(schedule);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ScheduleDto>(schedule);
        }

        public async Task UpdateAsync(UpdateScheduleDto updateDto, int doctorId)
        {
            var schedule = await _unitOfWork.DoctorSchedules.GetByIdAsync(updateDto.Id);

            if (schedule == null) throw new Exception("Kayıt bulunamadı.");

            if (schedule.DoctorId != doctorId)
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmaya yetkiniz yok.");
            }
            
            if (updateDto.StartTime >= updateDto.EndTime)
            {
                throw new ArgumentException("Başlangıç saati, bitiş saatinden önce olmalıdır.");
            }

        
            _mapper.Map(updateDto, schedule);

            _unitOfWork.DoctorSchedules.Update(schedule);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id, int doctorId)
        {
            var schedule = await _unitOfWork.DoctorSchedules.GetByIdAsync(id);

            if (schedule == null) throw new Exception("Kayıt bulunamadı.");

            
            if (schedule.DoctorId != doctorId)
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmaya yetkiniz yok.");
            }

            _unitOfWork.DoctorSchedules.Remove(schedule);
            await _unitOfWork.CompleteAsync();
        }
    }
}