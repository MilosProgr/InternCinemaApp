using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Application.DTO.ReservationsDTO.ReservationDTO
{
    public class UpdateReservationDTO
    {
        //[Required]
        //public int Id { get; set; }

        public bool IsCancelled { get; set; }
    }
}
