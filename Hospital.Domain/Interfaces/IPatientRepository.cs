using Hospital.Domain.Entities;
using System.Threading.Tasks;


namespace Hospital.Domain.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient?> GetByTcKimlikNoAsync(string tcKimlikNo);
    }
}