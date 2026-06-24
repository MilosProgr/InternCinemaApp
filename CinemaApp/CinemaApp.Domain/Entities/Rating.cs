using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Domain.Entities
{
    public class Rating
    {
        public int Id { get; set; }



        public int UserId { get; set; }

        public User User { get; set; } = null!;



        public int MovieId { get; set; }

        public Movie Movie { get; set; } = null!;



        [Range(1, 5)]
        public int Stars { get; set; }



        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
