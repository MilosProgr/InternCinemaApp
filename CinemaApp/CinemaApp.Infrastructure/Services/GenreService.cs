using CinemaApp.Infrastructure.Database;
using CinemaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Application.Services.Genres;
using CinemaApp.Application.Common.Models;

namespace CinemaApp.Services.Genres
{
    public class GenreService : IGenreService
    {
        private readonly AppDbContext _context;

        public GenreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Genre>> GetAll()
        {
            return await _context.Genres
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        public async Task<Genre?> GetById(int id)
        {
            return await _context.Genres
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Genre?> Create(Genre genre)
        {
            bool exists = await _context.Genres
                .AnyAsync(g => g.Name.ToLower() == genre.Name.ToLower());

            if (exists)
                return null;

            _context.Genres.Add(genre);

            await _context.SaveChangesAsync();

            return genre;
        }

        public async Task<Genre?> Update(int id, Genre genre)
        {
            var existingGenre = await _context.Genres
                .FirstOrDefaultAsync(g => g.Id == id);

            if (existingGenre == null)
                return null;

            bool exists = await _context.Genres
                .AnyAsync(g =>
                    g.Id != id &&
                    g.Name.ToLower() == genre.Name.ToLower());

            if (exists)
                return null;

            existingGenre.Name = genre.Name;

            await _context.SaveChangesAsync();

            return existingGenre;
        }

        public async Task<bool> Delete(int id)
        {
            var genre = await _context.Genres
                .FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null)
                return false;

            _context.Genres.Remove(genre);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PagedResult<Genre>> GetPaged(int page, int pageSize)
        {
            var total = await _context.Genres.CountAsync();
            var items = await _context.Genres
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Genre>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}