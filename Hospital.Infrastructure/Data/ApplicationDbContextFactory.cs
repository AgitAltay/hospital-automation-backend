using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection; // Assembly için gerekli

namespace Hospital.Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            // Bu, ApplicationDbContextFactory'nin bulunduğu assembly'nin yolunu verir (Infrastructure projesinin bin klasörü).
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var currentAssemblyDirectory = Path.GetDirectoryName(assemblyLocation);

            // Çözüm kök dizinini bulmak için, Infrastructure projesinin çıktısından geriye doğru gidelim.
            // Varsayım: "Infrastructure/bin/Debug/netX.0" -> "Infrastructure" -> "Solution Root"
            // Bu 2 seviye yukarı çıkmaktır.
            var solutionRoot = Path.Combine(currentAssemblyDirectory!, "..", "..", "..");

            // Hospital.API projesinin çıktı dizinini oluşturalım.
            // Örneğin: HospitalAutomation/Hospital.API/bin/Debug/net8.0
            var targetFramework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription
                                        .Replace(".NET ", "net").Split(" ")[0].ToLower(); // örn: net8.0

            string baseAppPath = Path.Combine(solutionRoot, "Hospital.API", "bin", environmentName, targetFramework);

            // DEBUG İÇİN KONSOLA YAZDIRMA (sorun olursa kontrol etmek için açabiliriz)
            // Console.WriteLine($"DEBUG: AppContext.BaseDirectory: {AppContext.BaseDirectory}");
            // Console.WriteLine($"DEBUG: Assembly Location: {assemblyLocation}");
            // Console.WriteLine($"DEBUG: Current Assembly Directory: {currentAssemblyDirectory}");
            // Console.WriteLine($"DEBUG: Solution Root: {solutionRoot}");
            // Console.WriteLine($"DEBUG: Computed API Output Path: {baseAppPath}");
            // Console.WriteLine($"DEBUG: Environment Name: {environmentName}");
            // Console.WriteLine($"DEBUG: Target Framework: {targetFramework}");


            if (!Directory.Exists(baseAppPath))
            {
                throw new InvalidOperationException($"Hospital.API projesinin çıktı dizini bulunamadı: {baseAppPath}. " +
                                                    "Lütfen yolun doğru olduğundan ve projenin derlenmiş olduğundan emin olun. " +
                                                    $"Current Working Directory: {Directory.GetCurrentDirectory()}");
            }

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(baseAppPath) // API projesinin çıktı dizinini kullan!
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Fallback olarak her zaman appsettings.json'ı oku
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"DefaultConnection connection string is not configured. " +
                                                    $"Please ensure 'ConnectionStrings:DefaultConnection' is defined in 'appsettings.json' " +
                                                    $"located in the Hospital.API project's output directory: {baseAppPath}.");
            }

            builder.UseNpgsql(connectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
}