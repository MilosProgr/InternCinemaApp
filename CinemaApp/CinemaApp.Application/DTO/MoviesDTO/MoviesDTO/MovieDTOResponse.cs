using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.GenresDTO;

namespace CinemaApp.Models.DTO.MoviesDTO.MoviesDTO
{
    public class MovieDTOResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string OriginalName { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string PosterUrl { get; set; } = string.Empty;
        public double AverageRating { get; set; }

        // umesto jednog GenreId — lista žanrova
        public List<GenreDTOResponse> Genres { get; set; } = new List<GenreDTOResponse>();

        public List<Link> Links { get; set; }
    }
}