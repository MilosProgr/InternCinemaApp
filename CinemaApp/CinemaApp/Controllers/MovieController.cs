using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.GenresDTO;
using CinemaApp.Application.DTO.MoviesDTO.MoviesDTO;
using CinemaApp.Application.Services.Movies;
using CinemaApp.Domain.Entities;
using CinemaApp.Models.DTO.MoviesDTO.MoviesDTO;
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

        // helper — mapira Movie entitet u response DTO
        private MovieDTOResponse MapToResponse(Movie movie)
        {
            return new MovieDTOResponse
            {
                Id = movie.Id,
                Name = movie.Name,
                OriginalName = movie.OriginalName,
                Duration = movie.Duration,
                PosterUrl = movie.PosterUrl,
                AverageRating = _movieService.GetAverageRating(movie),
                Genres = movie.MovieGenres
                    .Select(mg => new GenreDTOResponse
                    {
                        Id = mg.Genre.Id,
                        Name = mg.Genre.Name
                    })
                    .ToList()
            };
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] char? letter = null)
        {
            var paged = await _movieService.GetPaged(page, pageSize, search, letter);

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Movie";

            var links = new List<Link>
            {
                new Link($"{baseUrl}?page={page}&pageSize={pageSize}", "self", "GET")
            };

            if (User.IsInRole("ADMIN"))
                links.Add(new Link(baseUrl, "create", "POST"));

            var result = new
            {
                items = paged.Items.Select(m =>
                {
                    var dto = MapToResponse(m);

                    dto.Links = MovieLinkBuilder.Build(
                        m,
                        baseUrl,
                        User
                    );

                    return dto;
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

            var result = MapToResponse(movie);

            result.Links = MovieLinkBuilder.Build(
                movie,
                baseUrl,
                User
            );

            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMovieDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = new Movie
            {
                Name = dto.Name,
                OriginalName = dto.OriginalName,
                Duration = dto.Duration,
                PosterUrl = dto.PosterUrl
            };

            var createdMovie = await _movieService.Create(movie, dto.GenreIds);

            if (createdMovie == null)
                return BadRequest("Jedan ili više žanrova ne postoje.");

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Movie";

            var result = MapToResponse(createdMovie);

            result.Links = MovieLinkBuilder.Build(
                createdMovie,
                baseUrl,
                User
            );

            return CreatedAtAction(nameof(GetById), new { id = createdMovie.Id }, result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMovieDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = new Movie
            {
                Name = dto.Name,
                OriginalName = dto.OriginalName,
                Duration = dto.Duration,
                PosterUrl = dto.PosterUrl
            };

            var updatedMovie = await _movieService.Update(id, movie, dto.GenreIds);

            if (updatedMovie == null)
                return NotFound("Film nije pronađen ili žanrovi ne postoje.");

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Movie";

            var result = MapToResponse(updatedMovie);

            result.Links = MovieLinkBuilder.Build(
                updatedMovie,
                baseUrl,
                User
            );

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