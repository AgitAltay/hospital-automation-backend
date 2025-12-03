using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;
using Hospital.Domain.Enums; // Role enum'ý burada
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Hospital.Application.Services;
using Hospital.Domain.Entities;
using Hospital.Domain.Constants;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Alan adý sizin kodunuzdaki gibi _authService olarak tanýmlandý.
        private readonly IAuthAppService _authService;

        // Dependency Injection (Constructor Injection)
        // Program.cs'te kaydettiðimiz IAuthAppService'i buraya enjekte ediyoruz.
        public AuthController(IAuthAppService authService)
        {
            _authService = authService;
        }

        // Register (Sadece Doktor Kaydý)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                // ARTIK HASTA KAYDI YOK. Sadece Doktor (ve ileride Admin) kaydý yapýlacak.
                // Gelen tüm kayýt isteklerini varsayýlan olarak "Doctor" rolüne atýyoruz.

                // Role.Doctor yerine Roles.Doctor kullanýyoruz.
                var userDto = await _authService.RegisterAsync(registerDto, Roles.Doctor);

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                // Hata mesajýný dönüyoruz
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Burada da _authService kullanýldý.
                var authResponse = await _authService.LoginAsync(loginDto);
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                // Kullanýcý bulunamadý veya þifre yanlýþ hatasý
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }
}