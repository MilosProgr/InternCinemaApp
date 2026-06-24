using CinemaApp.Application.DTO.MovieGenreDTO;
using CinemaApp.Application.Services.MovieGenres;
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
    public class MovieGenreService : IMovieGenreService
    {
        private readonly AppDbContext _context;

        public MovieGenreService(AppDbContext context)
        {
            _context = context;
        }

        // Svi žanrovi za jedan film
        public async Task<IEnumerable<MovieGenreResponseDTO>> GetByMovieIdAsync(int movieId)
        {
            var movieExists = await _context.Movies.AnyAsync(m => m.Id == movieId);
            if (!movieExists)
                throw new KeyNotFoundException($"Film sa ID {movieId} ne postoji.");

            return await _context.MovieGenres
                .Where(mg => mg.MovieId == movieId)
                .Include(mg => mg.Movie)
                .Include(mg => mg.Genre)
                .Select(mg => new MovieGenreResponseDTO
                {
                    MovieId = mg.MovieId,
                    MovieName = mg.Movie.Name,
                    GenreId = mg.GenreId,
                    GenreName = mg.Genre.Name
                })
                .ToListAsync();
        }

        // Dodaj jedan žanr filmu
        public async Task<MovieGenreResponseDTO> AddGenreToMovieAsync(MovieGenreCreateDTO dto)
        {
            var movie = await _context.Movies.FindAsync(dto.MovieId)
                ?? throw new KeyNotFoundException($"Film sa ID {dto.MovieId} ne postoji.");

            var genre = await _context.Genres.FindAsync(dto.GenreId)
                ?? throw new KeyNotFoundException($"Žanr sa ID {dto.GenreId} ne postoji.");

            var alreadyExists = await _context.MovieGenres
                .AnyAsync(mg => mg.MovieId == dto.MovieId && mg.GenreId == dto.GenreId);

            if (alreadyExists)
                throw new InvalidOperationException("Film već ima ovaj žanr.");

            var movieGenre = new MovieGenre
            {
                MovieId = dto.MovieId,
                GenreId = dto.GenreId
            };

            _context.MovieGenres.Add(movieGenre);
            await _context.SaveChangesAsync();

            return new MovieGenreResponseDTO
            {
                MovieId = movie.Id,
                MovieName = movie.Name,
                GenreId = genre.Id,
                GenreName = genre.Name
            };
        }

        // Zameni sve žanrove filma novim spiskom
        public async Task UpdateMovieGenresAsync(MovieGenreUpdateDTO dto)
        {
            var movie = await _context.Movies.FindAsync(dto.MovieId)
                ?? throw new KeyNotFoundException($"Film sa ID {dto.MovieId} ne postoji.");

            // Proveri da li svi žanrovi postoje
            var genres = await _context.Genres
                .Where(g => dto.GenreIds.Contains(g.Id))
                .ToListAsync();

            if (genres.Count != dto.GenreIds.Count)
                throw new KeyNotFoundException("Jedan ili više žanrova ne postoje.");

            // Obriši stare
            var existing = await _context.MovieGenres
                .Where(mg => mg.MovieId == dto.MovieId)
                .ToListAsync();

            _context.MovieGenres.RemoveRange(existing);

            // Dodaj nove
            var newGenres = dto.GenreIds.Select(genreId => new MovieGenre
            {
                MovieId = dto.MovieId,
                GenreId = genreId
            });

            await _context.MovieGenres.AddRangeAsync(newGenres);
            await _context.SaveChangesAsync();
        }

        // Ukloni jedan žanr sa filma
        public async Task RemoveGenreFromMovieAsync(int movieId, int genreId)
        {
            var movieGenre = await _context.MovieGenres
                .FirstOrDefaultAsync(mg => mg.MovieId == movieId && mg.GenreId == genreId)
                ?? throw new KeyNotFoundException("Veza između filma i žanra ne postoji.");

            _context.MovieGenres.Remove(movieGenre);
            await _context.SaveChangesAsync();
        }
    }
}
