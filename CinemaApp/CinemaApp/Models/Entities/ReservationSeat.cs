namespace CinemaApp.Models.Entities
{
    public class ReservationSeat
    {
        public int Id { get; set; }


        public int ReservationId { get; set; }

        public Reservation Reservation { get; set; } = null!;



        public string SeatNumber { get; set; } = string.Empty;
    }
}
