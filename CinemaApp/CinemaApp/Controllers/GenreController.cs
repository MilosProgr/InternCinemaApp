using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.GenresDTO;
using CinemaApp.Application.Services.Genres;
using CinemaApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var paged = await _genreService.GetPaged(page, pageSize);

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Genre";


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
                    new Link(
                        $"{baseUrl}",
                        "create",
                        "POST"));
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


            var result = new
            {
                items = paged.Items.Select(g => new GenreDTOResponse
                {
                    Id = g.Id,
                    Name = g.Name,

                    Links = GenreLinkBuilder.Build(
                        g,
                        baseUrl,
                        User)
                }),

                paged.TotalCount,
                paged.Page,
                paged.PageSize,
                paged.TotalPages,

                Links = collectionLinks
            };


            return Ok(result);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var genre = await _genreService.GetById(id);

            if (genre == null)
                return NotFound();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Genre";


            var result = new GenreDTOResponse
            {
                Id = genre.Id,
                Name = genre.Name,

                Links = GenreLinkBuilder.Build(
                    genre,
                    baseUrl,
                    User)
            };


            return Ok(result);
        }



        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateGenreDTO genreDto)
        {
            var genre = new Genre
            {
                Name = genreDto.Name
            };


            var created = await _genreService.Create(genre);

            if (created == null)
                return BadRequest();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Genre";


            var result = new GenreDTOResponse
            {
                Id = created.Id,
                Name = created.Name,

                Links = GenreLinkBuilder.Build(
                    created,
                    baseUrl,
                    User)
            };


            return Ok(result);
        }



        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateGenreDTO genreDto)
        {
            var genre = new Genre
            {
                Id = id,
                Name = genreDto.Name
            };


            var updated = await _genreService.Update(id, genre);

            if (updated == null)
                return NotFound();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Genre";


            var result = new GenreDTOResponse
            {
                Id = updated.Id,
                Name = updated.Name,

                Links = GenreLinkBuilder.Build(
                    updated,
                    baseUrl,
                    User)
            };


            return Ok(result);
        }



        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _genreService.Delete(id);

            if (!deleted)
                return NotFound();


            return NoContent();
        }
    }
}