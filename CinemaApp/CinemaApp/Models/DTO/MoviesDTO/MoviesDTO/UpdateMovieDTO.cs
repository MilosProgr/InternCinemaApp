using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models.DTO.MoviesDTO.MoviesDTO
{
    public class UpdateMovieDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string OriginalName { get; set; } = string.Empty;

        [Range(1, 1000)]
        public int Duration { get; set; }

        public string PosterUrl { get; set; } = string.Empty;

        [Required]
        public int GenreId { get; set; }
    }
}
