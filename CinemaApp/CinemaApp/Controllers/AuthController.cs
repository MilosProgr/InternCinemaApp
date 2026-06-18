using CinemaApp.Application.DTO.AuthDTO.Login;
using CinemaApp.Application.DTO.AuthDTO.PasswordManagement;
using CinemaApp.Application.DTO.AuthDTO.Register;
using CinemaApp.Application.Services.Auth;
using CinemaApp.Domain.Entities;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CinemaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IAntiforgery _antiforgery;
        public AuthController(IAuthService auth, IAntiforgery antiforgery)
        {
            _auth = auth;
            _antiforgery = antiforgery;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var result = await _auth.Register(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _auth.Login(dto);
            if (result == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO dto)
        {
            await _auth.ForgotPassword(dto);
            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO dto)
        {
            await _auth.ResetPassword(dto);
            return Ok();
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _auth.ChangePassword(userId, dto);
            return Ok();
        }

        [HttpGet("csrf-token")]
        public IActionResult GetToken()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);

            return Ok(new
            {
                token = tokens.RequestToken
            });
        }
    }
}
