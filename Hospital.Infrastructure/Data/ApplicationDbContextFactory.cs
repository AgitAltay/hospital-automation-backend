using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Hospital.Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // 1. Çalışma dizinini al (Örn: C:\Proje\HospitalAutomation)
            var currentDirectory = Directory.GetCurrentDirectory();

            // 2. API projesinin yolunu bulmaya çalış.
            // İlk olarak, mevcut dizinin içinde "Hospital.API" klasörü var mı diye bak.
            var apiProjectPath = Path.Combine(currentDirectory, "Hospital.API");

            // Eğer mevcut dizinde yoksa (belki komut bir alt klasörden çalıştırılmıştır),
            // bir üst dizine çıkıp orada ara.
            if (!Directory.Exists(apiProjectPath))
            {
                var parentDir = Directory.GetParent(currentDirectory)?.FullName;
                if (parentDir != null)
                {
                    apiProjectPath = Path.Combine(parentDir, "Hospital.API");
                }
            }

            // 3. Güvenlik kontrolü: API klasörü bulundu mu?
            if (!Directory.Exists(apiProjectPath) || !File.Exists(Path.Combine(apiProjectPath, "appsettings.json")))
            {
                throw new DirectoryNotFoundException(
                    $"API projesi veya 'appsettings.json' dosyası bulunamadı!\n" +
                    $"Aranan yol: '{apiProjectPath}'\n" +
                    "Lütfen 'dotnet ef' komutunu ana çözüm klasöründen (HospitalAutomation.sln dosyasının olduğu yer) çalıştırdığınızdan emin olun.");
            }

            Console.WriteLine($"[Factory Info] Konfigürasyon dosyaları '{apiProjectPath}' klasöründen okunuyor...");

            // 4. Konfigürasyonu inşa et
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .Build();

            // 5. Bağlantı dizesini al ve DbContext'i oluştur
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"'DefaultConnection' bağlantı dizesi appsettings.json dosyasında bulunamadı.");
            }

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseNpgsql(connectionString, b => b.MigrationsAssembly("Hospital.Infrastructure"));

            return new ApplicationDbContext(builder.Options);
        }
    }
}