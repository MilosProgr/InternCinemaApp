using CinemaApp.Application.DTO.GenresDTO;

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


        public GenreDTOResponse Genre { get; set; } = null!;
    }
}
