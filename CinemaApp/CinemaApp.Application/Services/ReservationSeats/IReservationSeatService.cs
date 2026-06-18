using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Services.ReservationSeats
{
    public interface IReservationSeatService
    {
        Task<List<ReservationSeat>> GetAll();

        Task<ReservationSeat?> GetById(int id);

        Task<ReservationSeat?> Create(ReservationSeat reservationSeat);

        Task<ReservationSeat?> Update(int id, ReservationSeat reservationSeat);

        Task<bool> Delete(int id);
    }
}
