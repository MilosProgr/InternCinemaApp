
using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.MoviesDTO.MovieScreeningsDTO;
using CinemaApp.Application.DTO.UsersDTO;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Application.DTO.ReservationsDTO.ReservationDTO
{
    public class ReservationDTOResponse
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public UserDTOResponse? User { get; set; }

        // za gosta
        public string? GuestEmail { get; set; }

        public int MovieScreeningId { get; set; }

        public MovieScreeningDTOResponse MovieScreening { get; set; } = null!;

        [Required]
        public string ReservationCode { get; set; } = Guid.NewGuid().ToString();


        public decimal TotalPrice { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public bool IsCancelled { get; set; }
        public List<Link> Links { get; set; }
    }
}
