
using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.ReservationsDTO.ReservationDTO;

namespace CinemaApp.Application.DTO.ReservationsDTO.ReservationSeatDTO
{

    public class ReservationSeatDTOResponse
    {
        public int Id { get; set; }


        public int ReservationId { get; set; }

        public ReservationDTOResponse Reservation { get; set; } = null!;


        public string SeatNumber { get; set; } = string.Empty;
        public List<Link> Links { get; set; }
    }
}
