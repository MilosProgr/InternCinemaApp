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
        public async Task<IActionResult> GetAll()
        {
            var genres = await _genreService.GetAll();

            var result = genres.Select(g => new GenreDTOResponse
            {
                
                Id = g.Id,
                Name = g.Name
            });

            return Ok(result);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var genre = await _genreService.GetById(id);
            if (genre == null)
            {
                return NotFound();
            }
            var result = new GenreDTOResponse
            {
                Id = genre.Id,
                Name = genre.Name
            };
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGenreDTO genreDto)
        {
            var genre = new Genre
            {
                Name = genreDto.Name

            };
            var result = new GenreDTOResponse
            {
                Name = genre.Name 
            };

            var created = await _genreService.Create(genre);
            if (created == null)
                return BadRequest();

            return Ok(result);

        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateGenreDTO genreDto)
        {
            var genre = new Genre
            {
                Id = id,
                Name = genreDto.Name
            };
            var updated = await _genreService.Update(id,genre);
            if (updated == null)
                return NotFound();
            var result = new GenreDTOResponse
            {
                Id = updated.Id,
                Name = updated.Name
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
