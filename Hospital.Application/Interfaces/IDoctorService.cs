using Hospital.Application.DTOs.DoctorDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();
        Task<List<DoctorDto>> GetBySpecialtyIdAsync(int specialtyId);
        Task<DoctorDto> GetByIdAsync(int id);
        Task CreateAsync(CreateDoctorDto createDto);
        Task UpdateAsync(UpdateDoctorDto updateDto);
        Task DeleteAsync(int id);
    }
}