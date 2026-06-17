
using CinemaApp.Models.DTO.ReservationsDTO.ReservationDTO;

namespace CinemaApp.Models.DTO.ReservationsDTO.ReservationSeatDTO
{

    public class ReservationSeatDTOResponse
    {
        public int Id { get; set; }


        public int ReservationId { get; set; }

        public ReservationDTOResponse Reservation { get; set; } = null!;


        public string SeatNumber { get; set; } = string.Empty;
    }
}
