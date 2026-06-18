using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Application.DTO.MoviesDTO.MovieScreeningsDTO

{
    public class UpdateMovieScreeningDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Range(1, 100000)]
        public decimal TicketPrice { get; set; }

        [Range(1, 1000)]
        public int AvailableSeats { get; set; }
    }
}
