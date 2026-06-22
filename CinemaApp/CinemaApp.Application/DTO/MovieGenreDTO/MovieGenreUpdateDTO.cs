using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.DTO.MovieGenreDTO
{
    public class MovieGenreUpdateDTO
    {
        [Required]
        public int MovieId { get; set; }

        // Novi spisak svih žanrova za taj film
        [Required]
        public List<int> GenreIds { get; set; } = new();
    }
}
