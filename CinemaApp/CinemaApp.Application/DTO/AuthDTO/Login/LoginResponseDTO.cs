using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.DTO.AuthDTO.Login
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = null!;

        public int UserId { get; set; }

        public string Username { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string FullName { get; set; } = null!;
    }
}
