using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces; 
using System;
using System.Threading.Tasks;

namespace Hospital.Application.Services.Implementations
{
    public class AuthAppService : IAuthAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public AuthAppService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }


        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
   
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return new AuthResponseDto { Success = false, Message = "Bu e-posta adresi zaten kullanılıyor." };
            }

            _authService.CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);


            var user = new User
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                Role = registerDto.Role, 
                Gender = registerDto.Gender,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return new AuthResponseDto
            {
                Success = true,
                Data = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                }
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {

            var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return new AuthResponseDto { Success = false, Message = "Kullanıcı bulunamadı." };
            }

            if (!_authService.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new AuthResponseDto { Success = false, Message = "Hatalı şifre." };
            }

            var token = _authService.CreateToken(user);

            return new AuthResponseDto
            {
                Success = true,
                Data = new
                {
                    Token = token,
                    Expiration = DateTime.Now.AddDays(7),
                    Role = user.Role.ToString(),
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                }
            };
        }
    }
}