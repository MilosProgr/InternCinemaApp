using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.MoviesDTO.MovieScreeningsDTO;
using CinemaApp.Application.Services.MovieScreenings;
using CinemaApp.Domain.Entities;
using CinemaApp.Models.DTO.MoviesDTO.MoviesDTO;
using iText.Kernel.Geom;
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

        public MovieScreeningController(IMovieScreeningService movieScreeningService)
        {
            _movieScreeningService = movieScreeningService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var paged = await _movieScreeningService.GetPaged(page, pageSize);

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/MovieScreening";

            var collectionLinks = new List<Link>
    {
        new Link(
            $"{baseUrl}?page={page}&pageSize={pageSize}",
            "self",
            "GET")
    };

            if (User.IsInRole("ADMIN"))
            {
                collectionLinks.Add(
                    new Link(baseUrl, "create", "POST"));
            }

            if (page > 1)
            {
                collectionLinks.Add(
                    new Link(
                        $"{baseUrl}?page={page - 1}&pageSize={pageSize}",
                        "prev",
                        "GET"));
            }

            if (page < paged.TotalPages)
            {
                collectionLinks.Add(
                    new Link(
                        $"{baseUrl}?page={page + 1}&pageSize={pageSize}",
                        "next",
                        "GET"));
            }

            return Ok(new
            {
                items = paged.Items.Select(x => new MovieScreeningDTOResponse
                {
                    Id = x.Id,
                    MovieId = x.MovieId,

                    Movie = x.Movie == null ? null : new MovieDTOResponse
                    {
                        Id = x.Movie.Id,
                        Name = x.Movie.Name,
                        OriginalName = x.Movie.OriginalName,
                        Duration = x.Movie.Duration,
                        PosterUrl = x.Movie.PosterUrl
                        
                    },

                    StartTime = x.StartTime,
                    TicketPrice = x.TicketPrice,
                    AvailableSeats = x.AvailableSeats,

                    Links = MovieScreeningLinkBuilder.Build(
                        x,
                        baseUrl,
                        User)
                }),

                paged.TotalCount,
                paged.Page,
                paged.PageSize,
                paged.TotalPages,

                Links = collectionLinks
            });
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var screening = await _movieScreeningService.GetById(id);

            if (screening == null)
                return NotFound();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/MovieScreening";


            return Ok(new MovieScreeningDTOResponse
            {
                Id = screening.Id,
                MovieId = screening.MovieId,

                Movie = screening.Movie == null ? null : new MovieDTOResponse
                {
                    Id = screening.Movie.Id,
                    Name = screening.Movie.Name,
                    OriginalName = screening.Movie.OriginalName,
                    Duration = screening.Movie.Duration,
                    PosterUrl = screening.Movie.PosterUrl,
                    //GenreId = screening.Movie.GenreId
                },

                StartTime = screening.StartTime,
                TicketPrice = screening.TicketPrice,
                AvailableSeats = screening.AvailableSeats,

                Links = MovieScreeningLinkBuilder.Build(
                    screening,
                    baseUrl,
                    User)
            });
        }

        [HttpGet("upcoming7days")]
        public async Task<IActionResult> GetUpcoming7Days(
            int? genreId,
            DateTime? date,
            string sortBy = "chronologically")
        {
            var result =
                await _movieScreeningService.GetUpcoming7Days(
                    genreId,
                    date,
                    sortBy
                );

            return Ok(result);
        }



        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateMovieScreeningDTO dto)
        {
            var screening = new MovieScreening
            {
                MovieId = dto.MovieId,
                StartTime = dto.StartTime,
                TicketPrice = dto.TicketPrice,
                AvailableSeats = dto.AvailableSeats
            };


            var created = await _movieScreeningService.Create(screening);


            if (created == null)
                return BadRequest();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/MovieScreening";


            return Ok(new MovieScreeningDTOResponse
            {
                Id = created.Id,
                MovieId = created.MovieId,
                StartTime = created.StartTime,
                TicketPrice = created.TicketPrice,
                AvailableSeats = created.AvailableSeats,

                Links = MovieScreeningLinkBuilder.Build(
                    created,
                    baseUrl,
                    User)
            });
        }



        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateMovieScreeningDTO dto)
        {
            var screening = new MovieScreening
            {
                Id = id,
                MovieId = dto.MovieId,
                StartTime = dto.StartTime,
                TicketPrice = dto.TicketPrice,
                AvailableSeats = dto.AvailableSeats
            };


            var updated = await _movieScreeningService.Update(id, screening);


            if (updated == null)
                return NotFound();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/MovieScreening";


            return Ok(new MovieScreeningDTOResponse
            {
                Id = updated.Id,
                MovieId = updated.MovieId,
                StartTime = updated.StartTime,
                TicketPrice = updated.TicketPrice,
                AvailableSeats = updated.AvailableSeats,

                Links = MovieScreeningLinkBuilder.Build(
                    updated,
                    baseUrl,
                    User)
            });
        }



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