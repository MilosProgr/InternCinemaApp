using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Services.Reservations
{
    public interface IReservationService
    {
        Task<List<Reservation>> GetAll();

        Task<Reservation?> GetById(int id);

        Task<Reservation?> Create(Reservation reservation);

        Task<Reservation?> Update(int id, Reservation reservation);

        Task<bool> Delete(int id);
    }
}
