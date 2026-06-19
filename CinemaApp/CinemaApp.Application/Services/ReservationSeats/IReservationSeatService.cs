using CinemaApp.Application.Common.Models;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Services.ReservationSeats
{
    public interface IReservationSeatService
    {
        Task<List<ReservationSeat>> GetAll();

        Task<PagedResult<ReservationSeat>> GetPaged(int page, int pageSize);

        Task<ReservationSeat?> GetById(int id);

        Task<ReservationSeat?> Create(ReservationSeat reservationSeat);

        Task<ReservationSeat?> Update(int id, ReservationSeat reservationSeat);

        Task<bool> Delete(int id);
    }
}
