using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Application.DTO.ReservationsDTO.ReservationDTO
{
    public class CreateReservationDTO
    {
        public string? GuestEmail { get; set; }

        [Required]
        public int MovieScreeningId { get; set; }

        // ID-evi ScreeningSeat koji su slobodni
        [Required]
        [MinLength(1, ErrorMessage = "Morate izabrati bar jedno sedište.")]
        public List<int> SeatIds { get; set; } = new();
    }
}
