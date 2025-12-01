using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Sadece Doktorlar eriþebilir
    [Authorize(Roles = "Doctor")]
    public class DoctorController : ControllerBase
    {
        // GET: api/Doctor/my-appointments
        [HttpGet("my-appointments")]
        public IActionResult GetMyAppointments()
        {
            // Token'dan giriþ yapan doktorun ID'sini alalým
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Gerçek uygulamada bu ID'yi kullanarak veritabanýndan o doktorun randevularýný çekeceðiz.
            return Ok(new
            {
                DoctorId = doctorId,
                Appointments = new[] { "Hasta: Ahmet Yýlmaz, Saat: 14:00", "Hasta: Ayþe Demir, Saat: 15:30" },
                Message = "Doktor paneli: Randevularýnýz listeleniyor."
            });
        }
    }
}