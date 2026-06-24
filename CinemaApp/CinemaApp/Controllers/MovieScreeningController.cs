using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.GenresDTO;
using CinemaApp.Application.DTO.MoviesDTO.MovieScreeningsDTO;
using CinemaApp.Application.Services.Movies;
using CinemaApp.Application.Services.MovieScreenings;
using CinemaApp.Application.Services.ScreeningSeats;
using CinemaApp.Domain.Entities;
using CinemaApp.Models.DTO.MoviesDTO.MoviesDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MovieScreeningController : ControllerBase
    {
        private readonly IMovieScreeningService _movieScreeningService;
        private readonly IMovieService _movieService;
        private readonly IScreeningSeatService _seatService;

        public MovieScreeningController(
            IMovieScreeningService movieScreeningService,
            IMovieService movieService,
            IScreeningSeatService seatService)
        {
            _movieScreeningService = movieScreeningService;
            _movieService = movieService;
            _seatService = seatService;
        }

        // ── Privatni helper da se ne ponavlja mapiranje ───────────────────
        private MovieScreeningDTOResponse MapToResponse(MovieScreening x, string baseUrl)
        {
            return new MovieScreeningDTOResponse
            {
                Id = x.Id,
                MovieId = x.MovieId,
                StartTime = x.StartTime,
                TicketPrice = x.TicketPrice,

                Movie = x.Movie == null ? null : new MovieDTOResponse
                {
                    Id = x.Movie.Id,
                    Name = x.Movie.Name,
                    OriginalName = x.Movie.OriginalName,
                    Duration = x.Movie.Duration,
                    PosterUrl = x.Movie.PosterUrl,
                    AverageRating = _movieService.GetAverageRating(x.Movie),
                    Genres = x.Movie.MovieGenres
                        .Select(mg => new GenreDTOResponse
                        {
                            Id = mg.Genre.Id,
                            Name = mg.Genre.Name
                        }).ToList()
                },

                Links = MovieScreeningLinkBuilder.Build(x, baseUrl, User)
            };
        }

        // ── GET /api/MovieScreening ───────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var paged = await _movieScreeningService.GetPaged(page, pageSize);

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/MovieScreening";

            var collectionLinks = new List<Link>
            {
                new Link($"{baseUrl}?page={page}&pageSize={pageSize}", "self", "GET")
            };

            if (User.IsInRole("ADMIN"))
                collectionLinks.Add(new Link(baseUrl, "create", "POST"));

            if (page > 1)
                collectionLinks.Add(new Link(
                    $"{baseUrl}?page={page - 1}&pageSize={pageSize}", "prev", "GET"));

            if (page < paged.TotalPages)
                collectionLinks.Add(new Link(
                    $"{baseUrl}?page={page + 1}&pageSize={pageSize}", "next", "GET"));

            return Ok(new
            {
                items = paged.Items.Select(x => MapToResponse(x, baseUrl)),
                paged.TotalCount,
                paged.Page,
                paged.PageSize,
                paged.TotalPages,
                Links = collectionLinks
            });
        }

        // ── GET /api/MovieScreening/{id} ──────────────────────────────────
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var screening = await _movieScreeningService.GetById(id);

            if (screening == null)
                return NotFound();

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/MovieScreening";

            return Ok(MapToResponse(screening, baseUrl));
        }

        // ── GET /api/MovieScreening/upcoming7days ─────────────────────────
        [AllowAnonymous]
        [HttpGet("upcoming7days")]
        public async Task<IActionResult> GetUpcoming7Days(
            [FromQuery] int? genreId,
            [FromQuery] DateTime? date,
            [FromQuery] string sortBy = "chronologically")
        {
            var result = await _movieScreeningService.GetUpcoming7Days(genreId, date, sortBy);

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/MovieScreening";

            return Ok(result.Select(x => MapToResponse(x, baseUrl)));
        }

        // ── POST /api/MovieScreening ──────────────────────────────────────
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateMovieScreeningDTO dto)
        {
            var screening = new MovieScreening
            {
                MovieId = dto.MovieId,
                StartTime = dto.StartTime,
                TicketPrice = dto.TicketPrice,
            };

            var created = await _movieScreeningService.Create(screening);

            if (created == null)
                return BadRequest();

            // Generiši 42 sedišta (7x6) za novu projekciju
            await _seatService.GenerateSeatsForScreeningAsync(created.Id);

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/MovieScreening";

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                MapToResponse(created, baseUrl));
        }

        // ── PUT /api/MovieScreening/{id} ──────────────────────────────────
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateMovieScreeningDTO dto)
        {
            var screening = new MovieScreening
            {
                Id = id,
                MovieId = dto.MovieId,
                StartTime = dto.StartTime,
                TicketPrice = dto.TicketPrice,
            };

            var updated = await _movieScreeningService.Update(id, screening);

            if (updated == null)
                return NotFound();

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/MovieScreening";

            return Ok(MapToResponse(updated, baseUrl));
        }

        // ── DELETE /api/MovieScreening/{id} ───────────────────────────────
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _movieScreeningService.Delete(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}