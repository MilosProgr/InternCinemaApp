using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Services.Ratings
{
    public interface IRatingService
    {
        Task<List<Rating>> GetAll();

        Task<Rating?> GetById(int id);

        Task<Rating?> Create(Rating rating);

        Task<Rating?> Update(int id, Rating rating);

        Task<bool> Delete(int id);
    }
}
