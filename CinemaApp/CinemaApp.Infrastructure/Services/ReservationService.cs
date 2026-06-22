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



        public async Task<Reservation?> Create(Reservation reservation, List<int> seatIds)
        {
            // Proveri da li screening postoji
            var screening = await _context.MovieScreenings
                .Include(ms => ms.Seats)
                .FirstOrDefaultAsync(ms => ms.Id == reservation.MovieScreeningId);

            if (screening == null) return null;

            // Uzmi tražena sedišta koja su slobodna
            var seats = screening.Seats
                .Where(s => seatIds.Contains(s.Id) && !s.IsOccupied)
                .ToList();

            // Ako nisu sva sedišta slobodna, odbij
            if (seats.Count != seatIds.Count) return null;

            // Max 5 karata
            if (seats.Count > 5) return null;

            // Izračunaj cenu
            decimal discount = reservation.UserId != null ? 0.95m : 1.0m;
            reservation.TotalPrice = screening.TicketPrice * seats.Count * discount;

            reservation.ReservationCode = Guid.NewGuid().ToString();
            reservation.CreatedAt = DateTime.UtcNow;
            reservation.IsCancelled = false;

            // Zauzmi sedišta
            foreach (var seat in seats)
            {
                seat.IsOccupied = true;
                seat.Reservation = reservation;
            }

            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            return reservation;
        }

        public async Task<Reservation?> Cancel(int id, int? userId, bool isAdmin)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Seats)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null) return null;

            // Korisnik može otkazati samo svoju rezervaciju
            if (!isAdmin && reservation.UserId != userId) return null;

            // Ne može se otkazati već otkazana
            if (reservation.IsCancelled) return null;

            reservation.IsCancelled = true;

            // Oslobodi sedišta
            foreach (var seat in reservation.Seats)
            {
                seat.IsOccupied = false;
                seat.ReservationId = null;
            }

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
                .Include(r => r.User)
                .Include(r => r.MovieScreening)
                    .ThenInclude(ms => ms.Movie)
                .Include(r => r.Seats)
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