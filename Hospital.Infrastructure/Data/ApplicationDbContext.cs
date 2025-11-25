using Hospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System; // DateTime için
using System.Threading; // CancellationToken için
using System.Threading.Tasks; // SaveChangesAsync için

namespace Hospital.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<PatientComplaint> PatientComplaints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Doctor" },
                new Role { Id = 3, Name = "Patient" }
            );

            // User - Role ilişkisi
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Doctor - Specialty ilişkisi
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Specialty)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecialtyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment - Doctor ilişkisi
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment - Patient (User) ilişkisi
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // PatientComplaint - Patient (User) ilişkisi
            modelBuilder.Entity<PatientComplaint>()
                .HasOne(pc => pc.Patient)
                .WithMany()
                .HasForeignKey(pc => pc.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Doctor'ın User'dan türetildiğini ve ayrı bir tabloya sahip olduğunu belirtme
            modelBuilder.Entity<Doctor>()
                .ToTable("Doctors");


            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Role>().HasQueryFilter(r => !r.IsDeleted);
            modelBuilder.Entity<Doctor>().HasQueryFilter(d => !d.IsDeleted);
            modelBuilder.Entity<Specialty>().HasQueryFilter(s => !s.IsDeleted); 
            modelBuilder.Entity<Appointment>().HasQueryFilter(a => !a.IsDeleted);
            modelBuilder.Entity<PatientComplaint>().HasQueryFilter(pc => !pc.IsDeleted);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted: // Eğer soft delete istiyorsak, silme durumunu yakalayalım
                        entry.State = EntityState.Modified; // Durumu Modified yap
                        entry.Entity.IsDeleted = true; // IsDeleted'ı true yap
                        entry.Entity.UpdatedDate = DateTime.UtcNow; // Güncelleme tarihini de set et
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}