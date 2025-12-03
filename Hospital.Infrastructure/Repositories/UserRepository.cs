using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hospital.Infrastructure.Repositories
{
    
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
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