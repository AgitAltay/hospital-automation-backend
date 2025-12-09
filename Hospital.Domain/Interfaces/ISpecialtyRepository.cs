using Hospital.Domain.Entities;
using System.Threading.Tasks;

namespace Hospital.Domain.Interfaces
{
    public interface ISpecialtyRepository : IGenericRepository<Specialty>
    {
       
        Task<bool> SpecialtyExistsAsync(string specialtyName);
    }
}