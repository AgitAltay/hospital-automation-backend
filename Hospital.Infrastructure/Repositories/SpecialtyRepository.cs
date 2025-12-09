using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hospital.Infrastructure.Repositories
{
    public class SpecialtyRepository : GenericRepository<Specialty>, ISpecialtyRepository
    {
        public SpecialtyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> SpecialtyExistsAsync(string specialtyName)
        {
                
            return await _context.Specialties
                .AnyAsync(s => s.Name.ToLower() == specialtyName.ToLower() && !s.IsDeleted);
        }
    }
}