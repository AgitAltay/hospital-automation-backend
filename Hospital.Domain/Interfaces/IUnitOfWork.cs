using System.Threading.Tasks;

namespace Hospital.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        Task<int> SaveChangesAsync();
    }
}