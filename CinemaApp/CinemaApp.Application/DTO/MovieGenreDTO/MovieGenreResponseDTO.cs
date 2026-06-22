using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.DTO.MovieGenreDTO
{
    public class MovieGenreResponseDTO
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; } = string.Empty;
        public int GenreId { get; set; }
        public string GenreName { get; set; } = string.Empty;
    }
}
