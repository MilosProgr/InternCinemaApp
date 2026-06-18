using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Application.DTO.SeatsDTO
{
    public class UpdateSeatDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Row { get; set; } = string.Empty;

        [Range(1, 100)]
        public int Number { get; set; }
    }
}
