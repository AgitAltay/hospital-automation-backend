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

        // Backing fields (Lazy loading için gizli alanlar)
        private IUserRepository? _users;
        private IGenericRepository<Doctor>? _doctors;
        private IGenericRepository<Specialty>? _specialties;
        private IAppointmentRepository? _appointments;
        private IGenericRepository<PatientComplaint>? _patientComplaints;
        private IPatientRepository? _patients;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        // Public properties (İstendiğinde oluşturulur - Lazy Loading)
        // DİKKAT: Users için özel repository kullanıyoruz.
        public IUserRepository Users => _users ??= new UserRepository(_context);
        
        public IAppointmentRepository Appointments => _appointments ??= new AppointmentRepository(_context);
        public IPatientRepository Patient { get; }

        public IGenericRepository<Doctor> Doctors => _doctors ??= new GenericRepository<Doctor>(_context);
        public IGenericRepository<Specialty> Specialties => _specialties ??= new GenericRepository<Specialty>(_context);
        public IGenericRepository<PatientComplaint> PatientComplaints => _patientComplaints ??= new GenericRepository<PatientComplaint>(_context);
        public IPatientRepository Patients => _patients ??= new PatientRepository(_context);

        // Değişiklikleri veritabanına kaydet
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Bellek temizliği
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}