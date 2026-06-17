using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models.DTO.GenresDTO
{
    public record CreateGenreDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
