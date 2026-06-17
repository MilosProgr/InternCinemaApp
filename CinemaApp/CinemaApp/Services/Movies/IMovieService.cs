using CinemaApp.Models.Entities;

namespace CinemaApp.Services.Movies
{
    public interface IMovieService
    {
        Task<List<Movie>> GetAll();

        Task<Movie?> GetById(int id);

        Task<Movie?> Create(Movie movie);

        Task<Movie?> Update(int id, Movie movie);

        Task<bool> Delete(int id);
    }
}
