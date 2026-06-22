using CinemaApp.Infrastructure.Database;
using CinemaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Application.Services.Movies;
using CinemaApp.Application.Common.Models;

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
                .Include(x => x.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(x => x.Ratings)
                .ToListAsync();
        }

        public async Task<Movie?> GetById(int id)
        {
            return await _context.Movies
                .Include(x => x.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(x => x.Ratings)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // genreIds dolazi iz request-a (multi-select sa frontenda)
        public async Task<Movie?> Create(Movie movie, List<int> genreIds)
        {
            // proveri da li svi žanrovi postoje
            var genres = await _context.Genres
                .Where(g => genreIds.Contains(g.Id))
                .ToListAsync();

            if (genres.Count != genreIds.Count)
                return null; // neki žanr ne postoji

            _context.Movies.Add(movie);

            // dodaj junction zapise
            foreach (var genre in genres)
            {
                movie.MovieGenres.Add(new MovieGenre
                {
                    Movie = movie,
                    Genre = genre
                });
            }

            await _context.SaveChangesAsync();

            return movie;
        }

        public async Task<Movie?> Update(int id, Movie movie, List<int> genreIds)
        {
            var existingMovie = await _context.Movies
                .Include(x => x.MovieGenres)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingMovie == null)
                return null;

            // proveri da li svi žanrovi postoje
            var genres = await _context.Genres
                .Where(g => genreIds.Contains(g.Id))
                .ToListAsync();

            if (genres.Count != genreIds.Count)
                return null;

            // osnovna polja
            existingMovie.Name = movie.Name;
            existingMovie.OriginalName = movie.OriginalName;
            existingMovie.Duration = movie.Duration;
            existingMovie.PosterUrl = movie.PosterUrl;

            // obrisi stare žanrove pa dodaj nove
            existingMovie.MovieGenres.Clear();

            foreach (var genre in genres)
            {
                existingMovie.MovieGenres.Add(new MovieGenre
                {
                    MovieId = existingMovie.Id,
                    GenreId = genre.Id
                });
            }

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

        public async Task<PagedResult<Movie>> GetPaged(int page, int pageSize, string? search = null, char? letter = null)
        {
            var query = _context.Movies
                .Include(x => x.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(x => x.Ratings)
                .AsQueryable();

            // filter po prvom slovu (admin list view iz taska)
            if (letter.HasValue)
                query = query.Where(x => x.Name.StartsWith(letter.Value.ToString()));

            // search po imenu
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => x.Name.Contains(search) || x.OriginalName.Contains(search));

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Movie>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        // pomocna metoda za homepage — prosecna ocena
        public double GetAverageRating(Movie movie)
        {
            if (movie.Ratings == null || !movie.Ratings.Any())
                return 0;

            return Math.Round(movie.Ratings.Average(r => r.Stars), 1);
        }
    }
}