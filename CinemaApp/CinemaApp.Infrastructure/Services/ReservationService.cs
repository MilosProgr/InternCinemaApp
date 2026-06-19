using CinemaApp.Infrastructure.Database;
using CinemaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Application.Services.Reservations;
using CinemaApp.Application.Common.Models;

namespace CinemaApp.Services.Reservations
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _context;

        public ReservationService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Reservation>> GetAll()
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.MovieScreening)
                    .ThenInclude(ms => ms.Movie)
                .Include(r => r.Seats)
                .ToListAsync();
        }



        public async Task<Reservation?> GetById(int id)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.MovieScreening)
                    .ThenInclude(ms => ms.Movie)
                .Include(r => r.Seats)
                .FirstOrDefaultAsync(r => r.Id == id);
        }



        public async Task<Reservation?> Create(Reservation reservation)
        {
            reservation.CreatedAt = DateTime.UtcNow;

            if (string.IsNullOrEmpty(reservation.ReservationCode))
            {
                reservation.ReservationCode = Guid.NewGuid().ToString();
            }


            await _context.Reservations.AddAsync(reservation);

            await _context.SaveChangesAsync();

            return reservation;
        }




        public async Task<Reservation?> Update(int id, Reservation reservation)
        {
            var existingReservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == id);


            if (existingReservation == null)
                return null;



            existingReservation.GuestEmail = reservation.GuestEmail;

            existingReservation.MovieScreeningId = reservation.MovieScreeningId;

            existingReservation.TotalPrice = reservation.TotalPrice;

            existingReservation.IsCancelled = reservation.IsCancelled;


            await _context.SaveChangesAsync();


            return existingReservation;
        }





        public async Task<bool> Delete(int id)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == id);


            if (reservation == null)
                return false;



            _context.Reservations.Remove(reservation);

            await _context.SaveChangesAsync();


            return true;
        }

        public async Task<PagedResult<Reservation>> GetPaged(int page, int pageSize)
        {
            var total = await _context.Reservations.CountAsync();
            var items = await _context.Reservations
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Reservation>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}