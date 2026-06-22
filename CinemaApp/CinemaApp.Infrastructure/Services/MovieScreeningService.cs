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
            Console.WriteLine("CREATE SCREENING POZVAN");
            var movieExists = await _context.Movies
                .AnyAsync(x => x.Id == movieScreen.MovieId);

            if (!movieExists)
                return null;

            var duplicate = await _context.MovieScreenings
                .AnyAsync(x => x.MovieId == movieScreen.MovieId
                   && x.StartTime == movieScreen.StartTime);

            if (duplicate) return null;

            // Generiši sedišta 7x6
            var rows = new[] { "A", "B", "C", "D", "E", "F", "G" };
            foreach (var row in rows)
                for (int n = 1; n <= 6; n++)
                    movieScreen.Seats.Add(new ScreeningSeat
                    {
                        Row = row,
                        Number = n,
                        IsOccupied = false
                    });


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
                .Include(x => x.Seats)
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

        public async Task<List<MovieScreening>> GetUpcoming7Days(
                int? genreId,
                DateTime? date,
                string sortBy)
        {
            var query = _context.MovieScreenings
                .Include(x => x.Movie)
                    .ThenInclude(m => m.MovieGenres)
                        
                .AsQueryable();



            // samo narednih 7 dana
            var today = DateTime.UtcNow;

            var endDate = today.AddDays(7);


            query = query.Where(x =>
                x.StartTime >= today &&
                x.StartTime <= endDate
            );



            // filter po žanru
            if (genreId.HasValue)
            {
                query = query.Where(x =>
                    x.Movie.MovieGenres
                        .Any(mg => mg.GenreId == genreId.Value)
                );
            }



            // filter po datumu
            if (date.HasValue)
            {
                var utcDate = DateTime.SpecifyKind(
                    date.Value,
                    DateTimeKind.Utc
                );

                query = query.Where(x =>
                    x.StartTime.Date == utcDate.Date
                );
            }



            if (sortBy == "alphabetically")
            {
                query = query.OrderBy(x =>
                    x.Movie.Name
                );
            }
            else
            {
                query = query.OrderBy(x =>
                    x.StartTime
                );
            }



            return await query.ToListAsync();
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