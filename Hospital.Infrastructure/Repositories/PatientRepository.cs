using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.Data; 
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hospital.Infrastructure.Repositories
{

    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {

        public PatientRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Patient?> GetByTcKimlikNoAsync(string tcKimlikNo)
        {

            return await _context.Patients
                .FirstOrDefaultAsync(p => p.TcKimlikNo == tcKimlikNo);
        }
    }
}