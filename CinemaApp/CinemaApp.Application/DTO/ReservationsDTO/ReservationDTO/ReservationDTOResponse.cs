
using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.MoviesDTO.MovieScreeningsDTO;
using CinemaApp.Application.DTO.UsersDTO;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Application.DTO.ReservationsDTO.ReservationDTO
{
    public class ReservationSeatDTO
    {
        public int Id { get; set; }
        public string Row { get; set; } = string.Empty;
        public int Number { get; set; }
    }

    public class ReservationDTOResponse
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public UserDTOResponse? User { get; set; }
        public string? GuestEmail { get; set; }
        public int MovieScreeningId { get; set; }
        public MovieScreeningDTOResponse? MovieScreening { get; set; }
        public string ReservationCode { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCancelled { get; set; }

        // Sedišta koja su rezervisana
        public List<ReservationSeatDTO> Seats { get; set; } = new();

        public List<Link> Links { get; set; } = new();
    }
}
