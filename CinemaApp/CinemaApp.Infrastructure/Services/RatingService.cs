using CinemaApp.Infrastructure.Database;
using CinemaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Application.Services.Ratings;

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
            await _context.Ratings.AddAsync(rating);

            await _context.SaveChangesAsync();

            return rating;
        }


        public async Task<Rating?> Update(int id, Rating rating)
        {
            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.Id == id);


            if (existingRating == null)
            {
                return null;
            }


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
    }
}