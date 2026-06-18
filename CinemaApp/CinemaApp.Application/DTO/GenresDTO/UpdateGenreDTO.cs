using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Application.DTO.GenresDTO
{
    public record UpdateGenreDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
