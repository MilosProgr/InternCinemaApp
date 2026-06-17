using CinemaApp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models.DTO.UsersDTO
{
    public record UserDTOResponse
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public Role Role { get; set; }

        public bool IsVerified { get; set; } = false;

        public bool IsBlocked { get; set; } = false;
    }
}
