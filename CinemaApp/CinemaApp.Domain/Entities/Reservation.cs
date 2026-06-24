using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }


        public int? UserId { get; set; }

        public User? User { get; set; }


        // za gosta
        public string? GuestEmail { get; set; }



        public int MovieScreeningId { get; set; }

        public MovieScreening MovieScreening { get; set; } = null!;



        [Required]
        public string ReservationCode { get; set; } = Guid.NewGuid().ToString();



        public decimal TotalPrice { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



        public bool IsCancelled { get; set; } = false;

        public ICollection<ScreeningSeat> Seats { get; set; } = new List<ScreeningSeat>();


        // public ICollection<ReservationSeat> Seats { get; set; }
        //   = new List<ReservationSeat>();

        // Sedista koja su rezervisana u ovoj rezervaciji
    }
}
