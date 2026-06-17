using CinemaApp.Models.DTO.MoviesDTO.MoviesDTO;
using CinemaApp.Models.DTO.UsersDTO;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models.DTO.RatingsDTO
{
    public class RatingDTOResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public UserDTOResponse User { get; set; } = null!;


        public int MovieId { get; set; }

        public MovieDTOResponse Movie { get; set; } = null!;



        [Range(1, 5)]
        public int Stars { get; set; }



        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
