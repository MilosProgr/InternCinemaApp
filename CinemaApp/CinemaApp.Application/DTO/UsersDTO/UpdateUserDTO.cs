using CinemaApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Application.DTO.UsersDTO
{
    public class UpdateUserDTO
    {
        [Required]
        public int Id { get; set; }


        [Required]
        public string Username { get; set; } = string.Empty;


        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;


        public string FirstName { get; set; } = string.Empty;


        public string LastName { get; set; } = string.Empty;


        [Required]
        public DateTime DateOfBirth { get; set; }


        public Role Role { get; set; }


        public bool IsBlocked { get; set; }

        public bool isVerified { get; set; }
    }
}
