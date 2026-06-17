using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models.DTO.ReservationsDTO.ReservationDTO
{
    public class CreateReservationDTO
    {
        public string? GuestEmail { get; set; }

        [Required]
        public int MovieScreeningId { get; set; }

        [Required]
        public List<string> SeatNumbers { get; set; } = new();
    }
}
