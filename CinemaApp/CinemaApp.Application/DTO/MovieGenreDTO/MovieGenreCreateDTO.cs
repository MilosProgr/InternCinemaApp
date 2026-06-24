using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.DTO.MovieGenreDTO
{
    public class MovieGenreCreateDTO
    {
        [Required]
        public int MovieId { get; set; }

        [Required]
        public int GenreId { get; set; }
    }
}
