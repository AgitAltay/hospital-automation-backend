using AutoMapper;
using Hospital.Application.DTOs.SpecialtyDTOs;
using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Hospital.Application.Services
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SpecialtyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SpecialtyDto>> GetAllAsync()
        {
            var specialties = await _unitOfWork.Specialties.GetAllAsync();
            return _mapper.Map<List<SpecialtyDto>>(specialties);
        }

        public async Task<SpecialtyDto> GetByIdAsync(int id)
        {
            var specialty = await _unitOfWork.Specialties.GetByIdAsync(id);
            if (specialty == null)
            {
                throw new Exception("Uzmanlık alanı bulunamadı.");
            }
            return _mapper.Map<SpecialtyDto>(specialty);
        }

        public async Task CreateAsync(CreateSpecialtyDto createDto)
        {
            var specialty = _mapper.Map<Specialty>(createDto);

            await _unitOfWork.Specialties.AddAsync(specialty);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(UpdateSpecialtyDto updateDto)
        {
            var specialty = await _unitOfWork.Specialties.GetByIdAsync(updateDto.Id);
            if (specialty == null)
            {
                throw new Exception("Güncellenecek uzmanlık alanı bulunamadı.");
            }

            _mapper.Map(updateDto, specialty);

            _unitOfWork.Specialties.Update(specialty);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var specialty = await _unitOfWork.Specialties.GetByIdAsync(id);
            if (specialty == null)
            {
                throw new Exception("Silinecek uzmanlık alanı bulunamadı.");
            }

            // Repository'deki Delete metodu soft delete (IsDeleted = true) yapacak şekilde ayarlanmıştı.
            _unitOfWork.Specialties.Remove(specialty);
            await _unitOfWork.CompleteAsync();
        }
    }
}