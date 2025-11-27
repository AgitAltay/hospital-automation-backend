using Hospital.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Hospital.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // DİKKAT: Burayı IGenericRepository<User> yerine IUserRepository yaptık.
        IUserRepository Users { get; }

        // Diğerleri generic kalabilir çünkü özel bir sorgu yapmıyoruz (şimdilik).
        IGenericRepository<Role> Roles { get; }
        IGenericRepository<Doctor> Doctors { get; }
        IGenericRepository<Specialty> Specialties { get; }
        IGenericRepository<Appointment> Appointments { get; }
        IGenericRepository<PatientComplaint> PatientComplaints { get; }

        Task<int> CompleteAsync();
    }
}