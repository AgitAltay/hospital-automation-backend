using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.Data;
using Hospital.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace Hospital.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IUserRepository? _users;
        private IDoctorRepository? _doctors;
        private ISpecialtyRepository? _specialties;
        private IAppointmentRepository? _appointments;
        private IGenericRepository<PatientComplaint>? _patientComplaints;
        private IPatientRepository? _patients;
        private IDoctorScheduleRepository? _doctorSchedules;
        

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IUserRepository Users => _users ??= new UserRepository(_context);
        public IAppointmentRepository Appointments => _appointments ??= new AppointmentRepository(_context);
        public ISpecialtyRepository Specialties => _specialties ??= new SpecialtyRepository(_context);
        public IDoctorRepository Doctors => _doctors ??= new DoctorRepository(_context);
        
        public IGenericRepository<PatientComplaint> PatientComplaints => _patientComplaints ??= new GenericRepository<PatientComplaint>(_context);
        
        public IPatientRepository Patients => _patients ??= new PatientRepository(_context);
        public IDoctorScheduleRepository DoctorSchedules => _doctorSchedules ??= new DoctorScheduleRepository(_context);        

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}