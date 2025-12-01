using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // DÝKKAT: Bu nitelik, bu controller'daki tüm metotlara
    // sadece "Admin" rolüne sahip kullanýcýlarýn eriþebileceðini belirtir.
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        // GET: api/Admin/dashboard-stats
        [HttpGet("dashboard-stats")]
        public IActionResult GetDashboardStats()
        {
            // Gerçek uygulamada burada veritabanýndan istatistikleri çekeceðiz.
            // Þimdilik test için sahte veri dönüyoruz.
            return Ok(new { TotalUsers = 150, TotalDoctors = 20, TotalAppointments = 500, Message = "Admin paneline hoþ geldiniz!" });
        }

        // Buraya sadece Adminlerin yapabileceði diðer iþlemler eklenecek.
        // Örn: Yeni doktor ekleme, kullanýcý silme vb.
    }
}