using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models.DTO.ReservationsDTO.ReservationSeatDTO
{
    public class CreateReservationSeatDTO
    {
        [Required]
        public int ReservationId { get; set; }

        [Required]
        [MaxLength(10)]
        public string SeatNumber { get; set; } = string.Empty;
    }
}
