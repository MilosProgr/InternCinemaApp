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
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        //private readonly IEmailService _email;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        public async Task<RegisterResponseDTO> Register(RegisterDTO dto)
        {
            // 1. provera username/email
            var exists = await _context.Users
                .AnyAsync(x => x.Username == dto.Username || x.Email == dto.Email);

            if (exists) //fix
            {
                throw new InvalidOperationException("Username or email already exists");
            }

            // 2. kreiranje usera
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Email = dto.Email,
                Role = Role.CONSUMER,
                IsVerified = true,
                IsBlocked = false,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

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
            // 1. find user
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == dto.Username);

            if (user == null)
                //throw new Exception("Invalid credentials");
                return null;


            // 2. verify password
            var isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isValid)
                //throw new Exception("Invalid credentials");
                return null;

            // 3. generate JWT
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
            //var user = await _context.Users
            //    .FirstOrDefaultAsync(x => x.Email == dto.Email);

            //if (user == null)
            //    return;

            //var token = Guid.NewGuid().ToString();

            //_context.PasswordResetTokens.Add(new PasswordResetToken
            //{
            //    UserId = user.Id,
            //    Token = token,
            //    ExpiresAt = DateTime.UtcNow.AddHours(1),
            //    IsUsed = false
            //});

            //await _context.SaveChangesAsync();

            //var resetLink = $"http://localhost:3000/reset-password.html?token={token}";

            //var body = $@"
            //    <h3>Password Reset</h3>
            //    <p>Klikni na link da resetuješ password:</p>
            //    <a href='{resetLink}'>Reset Password</a>
            //    <p>Ako nisi ti tražio, ignoriši ovaj email.</p>
            //";

            //await _email.SendEmailAsync(
            //    user.Email,
            //    "Reset Password Request",
            //    body
            //);
        }

        public async Task ResetPassword(ResetPasswordDTO dto)
        {
            //var reset = await _context.PasswordResetTokens
            //    .Include(x => x.User)
            //    .FirstOrDefaultAsync(x =>
            //        x.Token == dto.Token &&
            //        !x.IsUsed);

            //if (reset == null)
            //    throw new Exception("Invalid token");

            //if (reset.ExpiresAt < DateTime.UtcNow)
            //    throw new Exception("Token expired");

            //reset.User.PasswordHash =
            //    BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            //reset.IsUsed = true;

            //await _context.SaveChangesAsync();
        }

        public async Task ChangePassword(int userId, ChangePasswordDTO dto)
        {
            //var user = await _context.Users.FindAsync(userId);

            //if (user == null)
            //    throw new Exception("User not found");

            //var isValid = BCrypt.Net.BCrypt.Verify(
            //    dto.CurrentPassword,
            //    user.PasswordHash);

            //if (!isValid)
            //    throw new Exception("Current password incorrect");

            //user.PasswordHash =
            //    BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            //await _context.SaveChangesAsync();
        }
    }
}
