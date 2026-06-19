using CinemaApp.Infrastructure.Database;
using CinemaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Application.Services.ReservationSeats;
using CinemaApp.Application.Common.Models;

namespace CinemaApp.Services.ReservationSeats
{
    public class ReservationSeatService : IReservationSeatService
    {
        private readonly AppDbContext _context;


        public ReservationSeatService(AppDbContext context)
        {
            _context = context;
        }



        public async Task<List<ReservationSeat>> GetAll()
        {
            return await _context.ReservationSeats
                .Include(x => x.Reservation)
                .ToListAsync();
        }



        public async Task<ReservationSeat?> GetById(int id)
        {
            return await _context.ReservationSeats
                .Include(x => x.Reservation)
                .FirstOrDefaultAsync(x => x.Id == id);
        }



        public async Task<ReservationSeat?> Create(ReservationSeat reservationSeat)
        {
            var reservationExists = await _context.Reservations
                .AnyAsync(x => x.Id == reservationSeat.ReservationId);


            if (!reservationExists)
            {
                return null;
            }


            await _context.ReservationSeats.AddAsync(reservationSeat);

            await _context.SaveChangesAsync();


            return reservationSeat;
        }



        public async Task<ReservationSeat?> Update(int id, ReservationSeat reservationSeat)
        {
            var existingSeat = await _context.ReservationSeats
                .FirstOrDefaultAsync(x => x.Id == id);


            if (existingSeat == null)
            {
                return null;
            }


            existingSeat.SeatNumber = reservationSeat.SeatNumber;
            existingSeat.ReservationId = reservationSeat.ReservationId;


            await _context.SaveChangesAsync();


            return existingSeat;
        }



        public async Task<bool> Delete(int id)
        {
            var seat = await _context.ReservationSeats
                .FirstOrDefaultAsync(x => x.Id == id);


            if (seat == null)
            {
                return false;
            }


            _context.ReservationSeats.Remove(seat);

            await _context.SaveChangesAsync();


            return true;
        }

        public async Task<PagedResult<ReservationSeat>> GetPaged(int page, int pageSize)
        {
            var total = await _context.ReservationSeats.CountAsync();
            var items = await _context.ReservationSeats
                .Include(x => x.Reservation)
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ReservationSeat>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}