using Hospital.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Domain.Interfaces
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<List<Doctor>> GetAllDoctorsWithSpecialtyAsync();

        Task<Doctor?> GetDoctorByIdWithSpecialtyAsync(int id);

        Task<List<Doctor>> GetDoctorsBySpecialtyIdAsync(int specialtyId);
    }
}