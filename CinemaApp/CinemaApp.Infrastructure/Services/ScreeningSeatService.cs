using CinemaApp.Application.Services.ScreeningSeats;
using CinemaApp.Domain.Entities;
using CinemaApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Infrastructure.Services
{
    public class ScreeningSeatService : IScreeningSeatService
    {
        private readonly AppDbContext _context;

        private static readonly string[] Rows = { "A", "B", "C", "D", "E", "F", "G" };
        private const int SeatsPerRow = 6;

        public ScreeningSeatService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ScreeningSeat>> GetByScreeningIdAsync(int screeningId)
        {
            return await _context.ScreeningSeats
                .Where(s => s.MovieScreeningId == screeningId)
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Number)
                .ToListAsync();
        }

        public async Task<IEnumerable<ScreeningSeat>> GetAvailableByScreeningIdAsync(int screeningId)
        {
            return await _context.ScreeningSeats
                .Where(s => s.MovieScreeningId == screeningId && !s.IsOccupied)
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Number)
                .ToListAsync();
        }

        public async Task GenerateSeatsForScreeningAsync(int screeningId)
        {
            bool alreadyExist = await _context.ScreeningSeats
                .AnyAsync(s => s.MovieScreeningId == screeningId);

            if (alreadyExist)
                throw new InvalidOperationException("Seats already generated for this screening.");

            var seats = new List<ScreeningSeat>();

            foreach (var row in Rows)
            {
                for (int number = 1; number <= SeatsPerRow; number++)
                {
                    seats.Add(new ScreeningSeat
                    {
                        MovieScreeningId = screeningId,
                        Row = row,
                        Number = number,
                        IsOccupied = false
                    });
                }
            }

            await _context.ScreeningSeats.AddRangeAsync(seats);
            await _context.SaveChangesAsync();
        }

        public async Task OccupySeatAsync(int seatId, int reservationId)
        {
            var seat = await _context.ScreeningSeats.FindAsync(seatId)
                ?? throw new KeyNotFoundException($"Seat {seatId} not found.");

            if (seat.IsOccupied)
                throw new InvalidOperationException($"Seat {seat.Row}{seat.Number} is already occupied.");

            seat.IsOccupied = true;
            seat.ReservationId = reservationId;

            await _context.SaveChangesAsync();
        }

        public async Task ReleaseSeatAsync(int seatId)
        {
            var seat = await _context.ScreeningSeats.FindAsync(seatId)
                ?? throw new KeyNotFoundException($"Seat {seatId} not found.");

            seat.IsOccupied = false;
            seat.ReservationId = null;

            await _context.SaveChangesAsync();
        }
    }
}
