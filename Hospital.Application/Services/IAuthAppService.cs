using Hospital.Application.DTOs;
using System.Threading.Tasks;

namespace Hospital.Application.Services
{
    public interface IAuthAppService
    {
        Task<AuthResponseDto?> Register(RegisterDto registerDto);
        Task<AuthResponseDto?> Login(LoginDto loginDto);
    }
}