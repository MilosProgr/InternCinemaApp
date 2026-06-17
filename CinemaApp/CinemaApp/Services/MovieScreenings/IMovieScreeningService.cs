using CinemaApp.Models.Entities;

namespace CinemaApp.Services.MovieScreenings
{
    public interface IMovieScreeningService
    {
        Task<List<MovieScreening>> GetAll();

        Task<MovieScreening?> GetById(int id);

        Task<MovieScreening?> Create(MovieScreening movieScreen);

        Task<MovieScreening?> Update(int id, MovieScreening movieScreen);

        Task<bool> Delete(int id);
    }
}
