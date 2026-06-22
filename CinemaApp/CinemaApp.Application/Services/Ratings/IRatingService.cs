using CinemaApp.Application.Common.Models;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Services.Ratings
{
    public interface IRatingService
    {
        Task<List<Rating>> GetAll();


        Task<PagedResult<Rating>> GetPaged(int page, int pageSize);

        Task<Rating?> GetById(int id);

        Task<Rating?> Create(Rating rating);

        Task<Rating?> Update(int id, int requestingUserId, Rating rating);

        Task<bool> Delete(int id);
    }
}
