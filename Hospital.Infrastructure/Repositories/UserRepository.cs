using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hospital.Infrastructure.Repositories
{
    // Bu sınıf hem GenericRepository'nin tüm yeteneklerini miras alır,
    // hem de IUserRepository'nin özel metotlarını uygular.
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            // Veritabanında e-postaya göre arama yap.
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByEmailWithRoleAsync(string email)
        {
            // E-postaya göre ararken "Include" ile Role tablosunu da çek.
            return await _dbSet
                .Include(u => u.Role) // JOIN işlemi
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<User>?> GetByFirstNameAsync(string username)
        {
            return await _dbSet.Include(u => u.FirstName).ToListAsync();
        }

        public async Task<List<User>?> GetByLastNameAsync(string username)
        {
            return await _dbSet.Include(u => u.LastName).ToListAsync();
        }

        public async Task<List<User>?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _dbSet.Include(u => u.PhoneNumber).ToListAsync();
        }
    }
}