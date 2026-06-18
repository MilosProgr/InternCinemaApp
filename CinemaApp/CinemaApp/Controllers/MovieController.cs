using CinemaApp.Application.Services.Movies;
using CinemaApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;


        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }



        // GET: api/Movie
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _movieService.GetAll();

            return Ok(movies);
        }



        // GET: api/Movie/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _movieService.GetById(id);


            if (movie == null)
            {
                return NotFound();
            }


            return Ok(movie);
        }



        // POST: api/Movie
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(Movie movie)
        {
            var createdMovie = await _movieService.Create(movie);


            if (createdMovie == null)
            {
                return BadRequest();
            }


            return CreatedAtAction(
                nameof(GetById),
                new { id = createdMovie.Id },
                createdMovie
            );
        }




        // PUT: api/Movie/5
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            Movie movie)
        {
            var updatedMovie = await _movieService.Update(id, movie);


            if (updatedMovie == null)
            {
                return NotFound();
            }


            return Ok(updatedMovie);
        }




        // DELETE: api/Movie/5
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _movieService.Delete(id);


            if (!result)
            {
                return NotFound();
            }


            return NoContent();
        }
    }
}