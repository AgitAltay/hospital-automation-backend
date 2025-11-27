using Hospital.Domain.Entities;
using System.Threading.Tasks;

namespace Hospital.Domain.Interfaces
{
    // IUserRepository, hem temel generic yeteneklere (IGenericRepository<User>) sahip olacak
    // hem de aşağıda tanımladığımız kendi özel metotlarına.
    public interface IUserRepository : IGenericRepository<User>
    {
        // E-posta adresine göre kullanıcıyı getir
        Task<User?> GetByEmailAsync(string email);

        // E-posta adresine göre kullanıcıyı, rol bilgisiyle (Include) birlikte getir
        Task<User?> GetByEmailWithRoleAsync(string email);
    }
}