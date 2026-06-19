using CinemaApp.Infrastructure.Database;
using CinemaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Application.Services.MovieScreenings;
using CinemaApp.Application.Common.Models;

namespace CinemaApp.Services.MovieScreenings
{
    public class MovieScreeningService : IMovieScreeningService
    {
        private readonly AppDbContext _context;


        public MovieScreeningService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<MovieScreening?> Create(MovieScreening movieScreen)
        {
            var movieExists = await _context.Movies
                .AnyAsync(x => x.Id == movieScreen.MovieId);

            if (!movieExists)
                return null;


            await _context.MovieScreenings.AddAsync(movieScreen);

            await _context.SaveChangesAsync();


            return movieScreen;
        }



        public async Task<bool> Delete(int id)
        {
            var screening = await _context.MovieScreenings
                .FirstOrDefaultAsync(x => x.Id == id);


            if (screening == null)
                return false;


            _context.MovieScreenings.Remove(screening);

            await _context.SaveChangesAsync();


            return true;
        }



        public async Task<List<MovieScreening>> GetAll()
        {
            return await _context.MovieScreenings
                .Include(x => x.Movie)
                .ToListAsync();
        }



        public async Task<MovieScreening?> GetById(int id)
        {
            return await _context.MovieScreenings
                .Include(x => x.Movie)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedResult<MovieScreening>> GetPaged(int page, int pageSize)
        {
            var total = await _context.MovieScreenings.CountAsync();
            var items = await _context.MovieScreenings
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<MovieScreening>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<MovieScreening?> Update(int id, MovieScreening movieScreen)
        {
            var existingScreening = await _context.MovieScreenings
                .FirstOrDefaultAsync(x => x.Id == id);


            if (existingScreening == null)
                return null;


            existingScreening.MovieId = movieScreen.MovieId;
            existingScreening.StartTime = movieScreen.StartTime;
            existingScreening.TicketPrice = movieScreen.TicketPrice;
            existingScreening.AvailableSeats = movieScreen.AvailableSeats;


            await _context.SaveChangesAsync();


            return existingScreening;
        }
    }
}