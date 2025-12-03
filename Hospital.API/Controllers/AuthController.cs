using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;
using Hospital.Domain.Enums; // Role enum'� burada
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
        // Alan ad� sizin kodunuzdaki gibi _authService olarak tan�mland�.
        private readonly IAuthAppService _authService;

        // Dependency Injection (Constructor Injection)
        // Program.cs'te kaydetti�imiz IAuthAppService'i buraya enjekte ediyoruz.
        public AuthController(IAuthAppService authService)
        {
            _authService = authService;
        }

        // Register (Sadece Doktor Kayd�)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                // ARTIK HASTA KAYDI YOK. Sadece Doktor (ve ileride Admin) kayd� yap�lacak.
                // Gelen t�m kay�t isteklerini varsay�lan olarak "Doctor" rol�ne at�yoruz.

                // Role.Doctor yerine Roles.Doctor kullan�yoruz.
                var userDto = await _authService.RegisterAsync(registerDto);

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                // Hata mesaj�n� d�n�yoruz
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Burada da _authService kullan�ld�.
                var authResponse = await _authService.LoginAsync(loginDto);
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                // Kullan�c� bulunamad� veya �ifre yanl�� hatas�
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }
}