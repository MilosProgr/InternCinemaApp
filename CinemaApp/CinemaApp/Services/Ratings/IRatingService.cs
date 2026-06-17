using CinemaApp.Models.Entities;

namespace CinemaApp.Services.Ratings
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
