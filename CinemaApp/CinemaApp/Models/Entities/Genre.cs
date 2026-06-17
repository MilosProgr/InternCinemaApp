using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models.Entities
{
    public class Genre
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; } = string.Empty;


        public ICollection<Movie> Movies { get; set; }
            = new List<Movie>();
    }
}
