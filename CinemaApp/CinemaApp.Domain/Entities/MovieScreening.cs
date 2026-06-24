namespace CinemaApp.Domain.Entities
{
    public class MovieScreening
    {
        public int Id { get; set; }


        public int MovieId { get; set; }

        public Movie Movie { get; set; } = null!;



        public DateTime StartTime { get; set; }


        public decimal TicketPrice { get; set; }


        //public int AvailableSeats { get; set; }


        public ICollection<ScreeningSeat> Seats { get; set; } = new List<ScreeningSeat>();

        public ICollection<Reservation> Reservations { get; set; }
            = new List<Reservation>();
    }
}
