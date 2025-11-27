using Hospital.Application.DTOs;
using System.Threading.Tasks;

namespace Hospital.Application.Services
{
    public interface IAuthAppService
    {
        // Kayıt olma metodu imzası
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

        // Giriş yapma metodu imzası
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    }
}