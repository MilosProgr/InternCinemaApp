using CinemaApp.Application.DTO.AuthDTO.Login;
using CinemaApp.Application.DTO.AuthDTO.PasswordManagement;
using CinemaApp.Application.DTO.AuthDTO.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> Login(LoginDTO dto);
        Task<RegisterResponseDTO> Register(RegisterDTO dto);

        Task ForgotPassword(ForgotPasswordDTO dto);
        Task ResetPassword(ResetPasswordDTO dto);
        Task ChangePassword(int userId, ChangePasswordDTO dto);
    }
}
