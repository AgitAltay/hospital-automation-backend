using Hospital.Application.DTOs.SpecialtyDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Application.Interfaces
{
    public interface ISpecialtyService
    {
        Task<List<SpecialtyDto>> GetAllAsync();

        Task<SpecialtyDto> GetByIdAsync(int id);

        Task CreateAsync(CreateSpecialtyDto createDto);

        Task UpdateAsync(UpdateSpecialtyDto updateDto);

        Task DeleteAsync(int id);
    }
}