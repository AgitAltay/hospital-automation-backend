using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;
using Hospital.Domain.Enums; // Role enum'� burada
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
using AutoMapper;
using Hospital.Application.Services;
using Hospital.Domain.Entities;
using LoggerManager.Interface;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Alan ad� sizin kodunuzdaki gibi _authService olarak tan�mland�.
        private readonly IAuthAppService _authService;
        private readonly ILoggerManager _logger;

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
                var userDto = await _authService.RegisterAsync(registerDto);
                _logger.LogInfo("User created successfully !");
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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