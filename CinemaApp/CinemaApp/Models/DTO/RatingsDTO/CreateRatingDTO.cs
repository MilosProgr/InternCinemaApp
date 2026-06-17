using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models.DTO.RatingsDTO
{
    public class CreateRatingDTO
    {
        [Required]
        public int MovieId { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }
    }
}
