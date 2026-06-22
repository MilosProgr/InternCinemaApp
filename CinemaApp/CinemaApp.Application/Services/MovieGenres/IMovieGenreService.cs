using CinemaApp.Application.DTO.MovieGenreDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.Services.MovieGenres
{
    public interface IMovieGenreService
    {
        Task<IEnumerable<MovieGenreResponseDTO>> GetByMovieIdAsync(int movieId);
        Task<MovieGenreResponseDTO> AddGenreToMovieAsync(MovieGenreCreateDTO dto);
        Task UpdateMovieGenresAsync(MovieGenreUpdateDTO dto);
        Task RemoveGenreFromMovieAsync(int movieId, int genreId);
    }
}
