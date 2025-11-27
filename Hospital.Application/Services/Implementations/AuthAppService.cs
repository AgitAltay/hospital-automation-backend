using Hospital.Application.DTOs;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Hospital.Application.Services.Implementations
{
    public class AuthAppService : IAuthAppService
    {
        // Domain katmanındaki UnitOfWork ve AuthService'i kullanacağız.
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        // Constructor Injection ile bağımlılıkları alıyoruz.
        public AuthAppService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        // --- KAYIT OLMA METODU ---
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // 1. İş Kuralı: Bu e-posta adresiyle kayıtlı bir kullanıcı var mı?
            // User repository'sini UnitOfWork üzerinden çağırıyoruz.
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return new AuthResponseDto { Success = false, Message = "Bu e-posta adresi zaten kullanılıyor." };
            }

            // 2. Şifreleme: Şifreyi hashle ve saltla.
            // Domain'deki AuthService bu işi yapar.
            _authService.CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // 3. Rol Kontrolü: Belirtilen rol var mı?
            var role = await _unitOfWork.Roles.GetByIdAsync(registerDto.RoleId);
            if (role == null)
            {
                return new AuthResponseDto { Success = false, Message = "Geçersiz rol ID'si." };
            }

            // 4. Yeni User entity'sini oluştur.
            var user = new User
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                RoleId = registerDto.RoleId,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            // 5. Veritabanına kaydet.
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync(); // Değişiklikleri veritabanına yansıt.

            // 6. Başarılı dönüş yap (DTO'ya dönüştür).
            return new AuthResponseDto
            {
                Success = true,
                Data = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleName = role.Name // Rol adını da ekliyoruz.
                }
            };
        }

        // --- GİRİŞ YAPMA METODU ---
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // 1. Kullanıcıyı bul.
            // User repository'sini UnitOfWork üzerinden çağırıyoruz.
            // Rolü de dahil ederek (Include) getiriyoruz ki token içine rol bilgisini koyabilelim.
            var user = await _unitOfWork.Users.GetByEmailWithRoleAsync(loginDto.Email);

            if (user == null)
            {
                return new AuthResponseDto { Success = false, Message = "Kullanıcı bulunamadı." };
            }

            // 2. Şifre Kontrolü: Girilen şifre hash'i, veritabanındakiyle eşleşiyor mu?
            if (!_authService.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new AuthResponseDto { Success = false, Message = "Hatalı şifre." };
            }

            // 3. JWT Token Üretimi.
            // Domain'deki AuthService bu işi yapar.
            var token = _authService.CreateToken(user);

            // 4. Başarılı dönüş yap (Token'ı string olarak döndür).
            return new AuthResponseDto
            {
                Success = true,
                Data = token
            };
        }
    }
}