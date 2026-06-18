using CinemaApp.Models.DTO.MoviesDTO.MoviesDTO;
using CinemaApp.Models.DTO.RatingsDTO;
using CinemaApp.Models.DTO.UsersDTO;
using CinemaApp.Models.Entities;
using CinemaApp.Services.Ratings;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;


        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ratings = await _ratingService.GetAll();


            return Ok(ratings.Select(r => new RatingDTOResponse
            {
                Id = r.Id,

                UserId = r.UserId,

                MovieId = r.MovieId,

                Stars = r.Stars,

                CreatedAt = r.CreatedAt,

                User = new UserDTOResponse
                {
                    Id = r.User.Id,
                    Username = r.User.Username,
                    FirstName = r.User.FirstName,
                    LastName = r.User.LastName,
                    Email = r.User.Email,
                    Role = r.User.Role
                },

                Movie = new MovieDTOResponse
                {
                    Id = r.Movie.Id,
                    Name = r.Movie.Name,
                    OriginalName = r.Movie.OriginalName,
                    Duration = r.Movie.Duration,
                    PosterUrl = r.Movie.PosterUrl
                }

            }));
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rating = await _ratingService.GetById(id);


            if (rating == null)
                return NotFound();



            return Ok(new RatingDTOResponse
            {
                Id = rating.Id,
                UserId = rating.UserId,
                MovieId = rating.MovieId,
                Stars = rating.Stars,
                CreatedAt = rating.CreatedAt
            });
        }



        [HttpPost]
        public async Task<IActionResult> Create(CreateRatingDTO dto)
        {

            var rating = new Rating
            {
                MovieId = dto.MovieId,
                Stars = dto.Stars,

                // privremeno
                UserId = 1
            };


            var created = await _ratingService.Create(rating);


            if (created == null)
                return BadRequest();



            return Ok(created);
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateRatingDTO dto)
        {

            var rating = new Rating
            {
                Id = id,
                Stars = dto.Stars
            };


            var updated = await _ratingService.Update(id, rating);



            if (updated == null)
                return NotFound();



            return Ok(updated);
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _ratingService.Delete(id);


            if (!deleted)
                return NotFound();


            return NoContent();
        }
    }
}