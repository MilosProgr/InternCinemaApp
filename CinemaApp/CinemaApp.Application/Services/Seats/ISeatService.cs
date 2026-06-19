using CinemaApp.Application.Common.Models;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Services.Seats
{
    public interface ISeatService
    {
        Task<List<Seat>> GetAll();

        Task<PagedResult<Seat>> GetPaged(int page, int pageSize);

        Task<Seat?> GetById(int id);

        Task<Seat?> Create(Seat user);

        Task<Seat?> Update(int id, Seat seat);

        Task<bool> Delete(int id);
    }
}
