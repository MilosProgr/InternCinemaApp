using CinemaApp.Application.Common.Models;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Services.MovieScreenings
{
    public interface IMovieScreeningService
    {
        Task<List<MovieScreening>> GetAll();

        Task<PagedResult<MovieScreening>> GetPaged(int page, int pageSize);

        Task<MovieScreening?> GetById(int id);

        Task<MovieScreening?> Create(MovieScreening movieScreen);

        Task<MovieScreening?> Update(int id, MovieScreening movieScreen);

        Task<List<MovieScreening>> GetUpcoming7Days(
            int? genreId,
            DateTime? date,
            string sortBy
        );

        Task<bool> Delete(int id);
    }
}
