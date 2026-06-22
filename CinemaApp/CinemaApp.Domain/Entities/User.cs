using CinemaApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public Role Role { get; set; }

        public bool IsVerified { get; set; } = false;

        public bool IsBlocked { get; set; } = false;

        public ICollection<Genre> FavoriteGenres { get; set; } = new List<Genre>();

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
