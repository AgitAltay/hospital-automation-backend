using Hospital.Domain.Entities;
using System.Threading.Tasks;

namespace Hospital.Domain.Interfaces
{

    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);

    }
}