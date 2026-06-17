using CinemaApp.Models.Entities;

namespace CinemaApp.Services.Seats
{
    public interface ISeatService
    {
        Task<List<Seat>> GetAll();

        Task<Seat?> GetById(int id);

        Task<Seat?> Create(Seat user);

        Task<Seat?> Update(int id, Seat seat);

        Task<bool> Delete(int id);
    }
}
