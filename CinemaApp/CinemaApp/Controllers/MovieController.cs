using CinemaApp.Application.Common.HATEOAS;
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



        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var paged = await _movieService.GetPaged(page, pageSize);


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Movie";


            var links = new List<Link>
            {
                new Link(
                    $"{baseUrl}?page={page}&pageSize={pageSize}",
                    "self",
                    "GET")
            };


            if (User.IsInRole("ADMIN"))
            {
                links.Add(
                    new Link(
                        $"{baseUrl}",
                        "create",
                        "POST")
                );
            }


            var result = new
            {
                items = paged.Items.Select(m => new
                {
                    m.Id,
                    m.Name,
                    m.OriginalName,
                    m.Duration,
                    m.PosterUrl,

                    Links = MovieLinkBuilder.Build(
                        m,
                        baseUrl,
                        User)
                }),

                paged.TotalCount,
                paged.Page,
                paged.PageSize,
                paged.TotalPages,

                Links = links
            };


            return Ok(result);
        }



        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _movieService.GetById(id);


            if (movie == null)
                return NotFound();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Movie";


            var result = new
            {
                movie.Id,
                movie.Name,
                movie.OriginalName,

                Links = MovieLinkBuilder.Build(
                    movie,
                    baseUrl,
                    User)
            };


            return Ok(result);
        }




        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(Movie movie)
        {
            var createdMovie = await _movieService.Create(movie);


            if (createdMovie == null)
                return BadRequest();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Movie";


            var result = new
            {
                createdMovie.Id,
                createdMovie.Name,
                createdMovie.OriginalName,

                Links = MovieLinkBuilder.Build(
                    createdMovie,
                    baseUrl,
                    User)
            };


            return CreatedAtAction(
                nameof(GetById),
                new { id = createdMovie.Id },
                result
            );
        }





        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            Movie movie)
        {
            var updatedMovie = await _movieService.Update(id, movie);


            if (updatedMovie == null)
                return NotFound();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Movie";


            var result = new
            {
                updatedMovie.Id,
                updatedMovie.Name,
                updatedMovie.OriginalName,

                Links = MovieLinkBuilder.Build(
                    updatedMovie,
                    baseUrl,
                    User)
            };


            return Ok(result);
        }




        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _movieService.Delete(id);


            if (!result)
                return NotFound();


            return NoContent();
        }
    }
}