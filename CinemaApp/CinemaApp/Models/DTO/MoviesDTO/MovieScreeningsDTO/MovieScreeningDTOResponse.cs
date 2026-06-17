using CinemaApp.Models.DTO.MoviesDTO.MoviesDTO;

namespace CinemaApp.Models.DTO.MoviesDTO.MovieScreeningsDTO
{
    public record MovieScreeningDTOResponse
    {
        public int Id { get; set; }


        public int MovieId { get; set; }

        public MovieDTOResponse? Movie { get; set; }



        public DateTime StartTime { get; set; }


        public decimal TicketPrice { get; set; }


        public int AvailableSeats { get; set; }
    }
}
