using CinemaApp.Database;
using CinemaApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Seats
{
    public class SeatService : ISeatService
    {
        private readonly AppDbContext _context;


        public SeatService(AppDbContext context)
        {
            _context = context;
        }



        public async Task<List<Seat>> GetAll()
        {
            return await _context.Seats
                .ToListAsync();
        }



        public async Task<Seat?> GetById(int id)
        {
            return await _context.Seats
                .FirstOrDefaultAsync(x => x.Id == id);
        }



        public async Task<Seat?> Create(Seat seat)
        {
            // provera da li sedište već postoji
            var exists = await _context.Seats
                .AnyAsync(x =>
                    x.Row == seat.Row &&
                    x.Number == seat.Number);


            if (exists)
            {
                return null;
            }


            await _context.Seats.AddAsync(seat);

            await _context.SaveChangesAsync();


            return seat;
        }



        public async Task<Seat?> Update(int id, Seat seat)
        {
            var existingSeat = await _context.Seats
                .FirstOrDefaultAsync(x => x.Id == id);


            if (existingSeat == null)
            {
                return null;
            }


            existingSeat.Row = seat.Row;
            existingSeat.Number = seat.Number;


            await _context.SaveChangesAsync();


            return existingSeat;
        }



        public async Task<bool> Delete(int id)
        {
            var seat = await _context.Seats
                .FirstOrDefaultAsync(x => x.Id == id);


            if (seat == null)
            {
                return false;
            }


            _context.Seats.Remove(seat);

            await _context.SaveChangesAsync();


            return true;
        }
    }
}