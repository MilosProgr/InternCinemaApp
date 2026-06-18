using CinemaApp.Application.DTO.MoviesDTO.MovieScreeningsDTO;
using CinemaApp.Models.DTO.MoviesDTO.MoviesDTO;
using CinemaApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using CinemaApp.Application.Services.MovieScreenings;

namespace CinemaApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class MovieScreeningController : ControllerBase
    {
        private readonly IMovieScreeningService _movieScreeningService;


        public MovieScreeningController(IMovieScreeningService movieScreeningService)
        {
            _movieScreeningService = movieScreeningService;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var screenings = await _movieScreeningService.GetAll();


            return Ok(screenings.Select(x => new MovieScreeningDTOResponse
            {
                Id = x.Id,
                MovieId = x.MovieId,
                Movie = x.Movie == null ? null : new MovieDTOResponse
                {
                    Id = x.Movie.Id,
                    Name = x.Movie.Name,
                    OriginalName = x.Movie.OriginalName,
                    Duration = x.Movie.Duration,
                    PosterUrl = x.Movie.PosterUrl,
                    GenreId = x.Movie.GenreId
                },
                StartTime = x.StartTime,
                TicketPrice = x.TicketPrice,
                AvailableSeats = x.AvailableSeats
            }));
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var screening = await _movieScreeningService.GetById(id);


            if (screening == null)
                return NotFound();


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
                    GenreId = screening.Movie.GenreId
                },
                StartTime = screening.StartTime,
                TicketPrice = screening.TicketPrice,
                AvailableSeats = screening.AvailableSeats
            });
        }



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


            return Ok(new MovieScreeningDTOResponse
            {
                Id = created.Id,
                MovieId = created.MovieId,
                StartTime = created.StartTime,
                TicketPrice = created.TicketPrice,
                AvailableSeats = created.AvailableSeats
            });
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateMovieScreeningDTO dto)
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


            return Ok(new MovieScreeningDTOResponse
            {
                Id = updated.Id,
                MovieId = updated.MovieId,
                StartTime = updated.StartTime,
                TicketPrice = updated.TicketPrice,
                AvailableSeats = updated.AvailableSeats
            });
        }



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