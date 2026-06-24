using CinemaApp.Infrastructure.Database;
using CinemaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Application.Services.Ratings;
using CinemaApp.Application.Common.Models;

namespace CinemaApp.Services.Ratings
{
    public class RatingService : IRatingService
    {
        private readonly AppDbContext _context;

        public RatingService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Rating>> GetAll()
        {
            return await _context.Ratings
                .Include(r => r.User)
                .Include(r => r.Movie)
                .ToListAsync();
        }


        public async Task<Rating?> GetById(int id)
        {
            return await _context.Ratings
                .Include(r => r.User)
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(r => r.Id == id);
        }


        public async Task<Rating?> Create(Rating rating)
        {
            // Proveri da li već postoji ocena za ovaj film od ovog korisnika
            var exists = await _context.Ratings
                .AnyAsync(r => r.UserId == rating.UserId && r.MovieId == rating.MovieId);

            if (exists)
                return null; // controller vraća BadRequest

            // U RatingService.Create() — dodaj nakon exists provere:
            var hasPastReservation = await _context.Reservations
                .AnyAsync(r => r.UserId == rating.UserId
                           && r.MovieScreening.MovieId == rating.MovieId
                           && r.MovieScreening.StartTime < DateTime.UtcNow
                           && !r.IsCancelled);

            if (!hasPastReservation)
                return null; // nije gledao film

            await _context.Ratings.AddAsync(rating);

            await _context.SaveChangesAsync();

            return rating;
        }


        public async Task<Rating?> Update(int id, int requestingUserId, Rating rating)
        {
            var existingRating = await _context.Ratings
       .FirstOrDefaultAsync(r => r.Id == id);

            if (existingRating == null) return null;

            // Samo vlasnik ili Admin može menjati
            if (existingRating.UserId != requestingUserId)
                return null; // controller vraća Forbid()

            existingRating.Stars = rating.Stars;
            await _context.SaveChangesAsync();
            return existingRating;
        }


        public async Task<bool> Delete(int id)
        {
            var rating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.Id == id);


            if (rating == null)
            {
                return false;
            }


            _context.Ratings.Remove(rating);

            await _context.SaveChangesAsync();


            return true;
        }

        public async Task<PagedResult<Rating>> GetPaged(int page, int pageSize)
        {
            var total = await _context.Ratings.CountAsync();

            var items = await _context.Ratings
                .Include(r => r.User)
                .Include(r => r.Movie)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Rating>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}