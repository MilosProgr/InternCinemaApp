using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Domain.Entities
{
    public class Genre
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;


        public ICollection<MovieGenre> MovieGenres { get; set; }
            = new List<MovieGenre>();
    }
}
