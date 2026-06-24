using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; } = string.Empty;


        public string OriginalName { get; set; } = string.Empty;


        public int Duration { get; set; }
        // minuta


        public string PosterUrl { get; set; } = string.Empty;


        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();


        public ICollection<MovieScreening> Screenings { get; set; }
            = new List<MovieScreening>();


        public ICollection<Rating> Ratings { get; set; }
            = new List<Rating>();
    }
}
