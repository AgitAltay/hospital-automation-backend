using Hospital.Application.DTOs;
using Hospital.Application.Services;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using System.Threading.Tasks;
using System.Linq; 

namespace Hospital.Application.Services.Implementations
{
    public class AuthAppService : IAuthAppService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthAppService(
            IGenericRepository<User> userRepository,
            IGenericRepository<Role> roleRepository,
            IAuthService authService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponseDto?> Register(RegisterDto registerDto)
        {
            if (await _userRepository.FindAsync(u => u.Email == registerDto.Email) is not null && (await _userRepository.FindAsync(u => u.Email == registerDto.Email)).Any())
            {
                return null; 
            }

            _authService.CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var role = (await _roleRepository.FindAsync(r => r.Name == registerDto.Role)).FirstOrDefault();
            if (role == null)
            {
             
                return null;
            }

            var user = new User
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RoleId = role.Id,
                Role = role 
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(); 

            var token = _authService.CreateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleName = role.Name
                }
            };
        }

        public async Task<AuthResponseDto?> Login(LoginDto loginDto)
        {
            var user = (await _userRepository.FindAsync(u => u.Email == loginDto.Email)).FirstOrDefault();
            if (user == null)
            {
                return null; 
            }

            
            if (user.Role == null)
            {
                user.Role = await _roleRepository.GetByIdAsync(user.RoleId);
            }

            if (!_authService.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return null; 
            }

            var token = _authService.CreateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleName = user.Role?.Name ?? "Unknown"
                }
            };
        }
    }
}