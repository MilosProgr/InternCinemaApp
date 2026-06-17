using CinemaApp.Database;
using CinemaApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Movies
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Movie>> GetAll()
        {
            return await _context.Movies
                .Include(x => x.Genre)
                .ToListAsync();
        }

        public async Task<Movie?> GetById(int id)
        {
            return await _context.Movies
                .Include(x => x.Genre)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Movie?> Create(Movie movie)
        {
            var genreExists = await _context.Genres
                .AnyAsync(x => x.Id == movie.GenreId);

            if (!genreExists)
                return null;

            _context.Movies.Add(movie);

            await _context.SaveChangesAsync();

            return movie;
        }

        public async Task<Movie?> Update(int id, Movie movie)
        {
            var existingMovie = await _context.Movies
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingMovie == null)
                return null;

            var genreExists = await _context.Genres
                .AnyAsync(x => x.Id == movie.GenreId);

            if (!genreExists)
                return null;

            existingMovie.Name = movie.Name;
            existingMovie.OriginalName = movie.OriginalName;
            existingMovie.Duration = movie.Duration;
            existingMovie.PosterUrl = movie.PosterUrl;
            existingMovie.GenreId = movie.GenreId;

            await _context.SaveChangesAsync();

            return existingMovie;
        }

        public async Task<bool> Delete(int id)
        {
            var movie = await _context.Movies
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null)
                return false;

            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}