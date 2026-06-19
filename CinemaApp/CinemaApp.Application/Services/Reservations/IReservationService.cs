using CinemaApp.Application.Common.Models;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Services.Reservations
{
    public interface IReservationService
    {
        Task<List<Reservation>> GetAll();

        Task<PagedResult<Reservation>> GetPaged(int page, int pageSize);

        Task<Reservation?> GetById(int id);

        Task<Reservation?> Create(Reservation reservation);

        Task<Reservation?> Update(int id, Reservation reservation);

        Task<bool> Delete(int id);
    }
}
