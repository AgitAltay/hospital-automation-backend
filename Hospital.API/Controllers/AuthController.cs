using Hospital.Application.DTOs;
using Hospital.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.API.Controllers
{
    // [Route("api/[controller]")] -> URL'in /api/Auth olmasýný saðlar.
    [Route("api/[controller]")]
    // [ApiController] -> Bu sýnýfýn bir API Controller olduðunu belirtir.
    // Otomatik model doðrulama (validation) gibi özellikleri aktif eder.
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthAppService _authService;

        // Dependency Injection (Constructor Injection)
        // Program.cs'te kaydettiðimiz IAuthAppService'i buraya enjekte ediyoruz.
        public AuthController(IAuthAppService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/register
        // Yeni kullanýcý kaydý için uç nokta.
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            // Application katmanýndaki servis metodunu çaðýr.
            var result = await _authService.RegisterAsync(registerDto);

            // Sonuç baþarýlýysa, 200 OK ve kayýt olan kullanýcý bilgisini dön.
            if (result.Success)
            {
                return Ok(result.Data);
            }

            // Sonuç baþarýsýzsa, 400 Bad Request ve hata mesajýný dön.
            return BadRequest(result.Message);
        }

        // POST: api/Auth/login
        // Kullanýcý giriþi ve token üretimi için uç nokta.
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            // Application katmanýndaki servis metodunu çaðýr.
            var result = await _authService.LoginAsync(loginDto);

            // Sonuç baþarýlýysa, 200 OK ve üretilen JWT Token'ý dön.
            if (result.Success)
            {
                return Ok(result.Data);
            }

            // Sonuç baþarýsýzsa (örn: yanlýþ þifre), 401 Unauthorized ve hata mesajýný dön.
            return Unauthorized(result.Message);
        }
    }
}