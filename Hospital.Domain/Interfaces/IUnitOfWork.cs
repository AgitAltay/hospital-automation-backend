using Hospital.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Hospital.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IAppointmentRepository Appointments { get; }
        IDoctorRepository Doctors { get; }
        ISpecialtyRepository Specialties { get; }

        IPatientRepository Patients { get; }
        IGenericRepository<PatientComplaint> PatientComplaints { get; }

        Task<int> CompleteAsync();
    }
}