using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Application.DTO.RatingsDTO

{
    public class UpdateRatingDTO
    {
        [Required]
        public int Id { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }
    }
}
