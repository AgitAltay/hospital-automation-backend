using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Rol fark etmeksizin, sadece giriþ yapmýþ olmak yeterli.
    [Authorize]
    public class UserController : ControllerBase
    {
        // GET: api/User/me
        // Giriþ yapmýþ kullanýcýnýn kendi bilgilerini döner.
        [HttpGet("me")]
        public IActionResult GetMyProfile()
        {
            // Token'daki claim'lerden bilgileri okuyoruz.
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Id = userId,
                Email = userEmail,
                Role = userRole,
                Message = "Bu sizin profil bilgilerinizdir."
            });
        }
    }
}