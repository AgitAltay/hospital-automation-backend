using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Infrastructure.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Doctor>> GetAllDoctorsWithSpecialtyAsync()
        {
            return await _context.Users
                .OfType<Doctor>() 
                .Include(d => d.Specialty) 
                .Where(d => !d.IsDeleted)
                .ToListAsync();
        }

        public async Task<Doctor?> GetDoctorByIdWithSpecialtyAsync(int id)
        {
            return await _context.Users
                .OfType<Doctor>()
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);
        }

        public async Task<List<Doctor>> GetDoctorsBySpecialtyIdAsync(int specialtyId)
        {
            return await _context.Users
                .OfType<Doctor>()
                .Include(d => d.Specialty)
                .Where(d => d.SpecialtyId == specialtyId && !d.IsDeleted)
                .ToListAsync();
        }
    }
}