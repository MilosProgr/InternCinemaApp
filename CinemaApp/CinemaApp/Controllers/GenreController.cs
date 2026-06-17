using CinemaApp.Models.DTO.GenresDTO;
using CinemaApp.Models.Entities;
using CinemaApp.Services.Genres;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
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
