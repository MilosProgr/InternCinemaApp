using CinemaApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models.DTO.MoviesDTO.MoviesDTO
{
    public class MovieDTOResponse
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; } = string.Empty;


        public string OriginalName { get; set; } = string.Empty;


        public int Duration { get; set; }
        // minuta


        public string PosterUrl { get; set; } = string.Empty;


        public int GenreId { get; set; }

        public Genre Genre { get; set; } = null!;
    }
}
