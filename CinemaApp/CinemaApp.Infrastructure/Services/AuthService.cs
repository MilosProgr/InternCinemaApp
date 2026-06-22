using BCrypt.Net;
using CinemaApp.Application.DTO.AuthDTO.Login;
using CinemaApp.Application.DTO.AuthDTO.PasswordManagement;
using CinemaApp.Application.DTO.AuthDTO.Register;
using CinemaApp.Application.Services.Auth;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Enums;
using CinemaApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CinemaApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        // private readonly IEmailService _email; // odkomentarisati kad se doda email servis

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<RegisterResponseDTO> Register(RegisterDTO dto)
        {
            // 1. Provera da li username ili email već postoje
            var exists = await _context.Users
                .AnyAsync(x => x.Username == dto.Username || x.Email == dto.Email);

            if (exists)
                throw new InvalidOperationException("Username or email already exists.");

            // 2. Kreiranje korisnika
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Email = dto.Email,
                Role = Role.CONSUMER,
                IsVerified = false,   // čeka verifikaciju emailom
                IsBlocked = false,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // TODO: ovde pošalji verification email kad se doda IEmailService

            return new RegisterResponseDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<LoginResponseDTO> Login(LoginDTO dto)
        {
            // 1. Pronađi korisnika po username-u ili emailu
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == dto.Username
                                       || x.Email == dto.Username);
            // dto.Username može biti i email — korisnik može da se loguje sa oba

            if (user == null)
                return null;

            // 2. Proveri lozinku
            var isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isValid)
                return null;

            // 3. Proveri verifikaciju
            if (!user.IsVerified)
                throw new InvalidOperationException("Account not verified. Please check your email.");

            // 4. Proveri blokiranost
            if (user.IsBlocked)
                throw new InvalidOperationException("Your account has been blocked.");

            // 5. Generiši JWT
            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                Token = token,
                UserId = user.Id,
                Username = user.Username,
                Role = user.Role.ToString(),
                FullName = user.FirstName + " " + user.LastName
            };
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task ForgotPassword(ForgotPasswordDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
                return; // ne otkrivamo da li email postoji — sigurnosna praksa

            var token = Guid.NewGuid().ToString();

            _context.PasswordResetTokens.Add(new PasswordResetToken
            {
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                IsUsed = false
            });

            await _context.SaveChangesAsync();

            // TODO: odkomentarisati kad se doda IEmailService
            // var resetLink = $"http://localhost:3000/reset-password.html?token={token}";
            // await _email.SendEmailAsync(user.Email, "Reset Password", $"<a href='{resetLink}'>Reset</a>");
        }

        public async Task ResetPassword(ResetPasswordDTO dto)
        {
            var reset = await _context.PasswordResetTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == dto.Token && !x.IsUsed);

            if (reset == null)
                throw new InvalidOperationException("Invalid token.");

            if (reset.ExpiresAt < DateTime.UtcNow)
                throw new InvalidOperationException("Token has expired.");

            reset.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            reset.IsUsed = true;

            await _context.SaveChangesAsync();
        }

        public async Task ChangePassword(int userId, ChangePasswordDTO dto)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                throw new InvalidOperationException("User not found.");

            var isValid = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash);

            if (!isValid)
                throw new InvalidOperationException("Current password is incorrect.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await _context.SaveChangesAsync();
        }
    }
}