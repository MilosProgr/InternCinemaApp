using CinemaApp.Application.DTO.MovieGenreDTO;
using CinemaApp.Application.Services.MovieGenres;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieGenreController : ControllerBase
    {
        private readonly IMovieGenreService _service;

        public MovieGenreController(IMovieGenreService service)
        {
            _service = service;
        }

        // GET api/moviegenre/movie/3
        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetByMovieId(int movieId)
        {
            try
            {
                var result = await _service.GetByMovieIdAsync(movieId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST api/moviegenre
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AddGenreToMovie([FromBody] MovieGenreCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.AddGenreToMovieAsync(dto);
                return CreatedAtAction(nameof(GetByMovieId),
                    new { movieId = result.MovieId }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // PUT api/moviegenre
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateMovieGenres([FromBody] MovieGenreUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.UpdateMovieGenresAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE api/moviegenre/movie/3/genre/2
        [HttpDelete("movie/{movieId}/genre/{genreId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RemoveGenreFromMovie(int movieId, int genreId)
        {
            try
            {
                await _service.RemoveGenreFromMovieAsync(movieId, genreId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
