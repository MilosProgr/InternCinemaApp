using CinemaApp.Application.Common.Models;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Services.Movies

{
    public interface IMovieService
    {
        Task<List<Movie>> GetAll();
        Task<Movie?> GetById(int id);
        Task<Movie?> Create(Movie movie, List<int> genreIds); // dodato genreIds
        Task<Movie?> Update(int id, Movie movie, List<int> genreIds); // dodato genreIds
        Task<bool> Delete(int id);
        Task<PagedResult<Movie>> GetPaged(int page, int pageSize, string? search = null, char? letter = null);
        double GetAverageRating(Movie movie);

    }
}
