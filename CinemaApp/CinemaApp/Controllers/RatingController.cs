using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.RatingsDTO;
using CinemaApp.Application.DTO.UsersDTO;
using CinemaApp.Application.Services.Ratings;
using CinemaApp.Domain.Entities;
using CinemaApp.Models.DTO.MoviesDTO.MoviesDTO;
using Microsoft.AspNetCore.Authorization;
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



        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1,[FromQuery] int pageSize = 10)
        {
            var paged = await _ratingService.GetPaged(page, pageSize);

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Rating";

            var collectionLinks = new List<Link>
    {
        new Link(
            $"{baseUrl}?page={page}&pageSize={pageSize}",
            "self",
            "GET")
    };

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
                items = paged.Items.Select(r => new RatingDTOResponse
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
                    },

                    Links = RatingLinkBuilder.Build(
                        r,
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




        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rating = await _ratingService.GetById(id);


            if (rating == null)
                return NotFound();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Rating";


            return Ok(new RatingDTOResponse
            {
                Id = rating.Id,

                UserId = rating.UserId,

                MovieId = rating.MovieId,

                Stars = rating.Stars,

                CreatedAt = rating.CreatedAt,


                Links = RatingLinkBuilder.Build(
                    rating,
                    baseUrl,
                    User)

            });
        }




        [Authorize(Roles = "CONSUMER,ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRatingDTO dto)
        {

            var rating = new Rating
            {
                MovieId = dto.MovieId,
                Stars = dto.Stars,

                // TODO: zameniti sa CurrentUserService
                UserId = 1
            };


            var created = await _ratingService.Create(rating);


            if (created == null)
                return BadRequest();



            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Rating";


            return Ok(new RatingDTOResponse
            {
                Id = created.Id,

                UserId = created.UserId,

                MovieId = created.MovieId,

                Stars = created.Stars,

                CreatedAt = created.CreatedAt,


                Links = RatingLinkBuilder.Build(
                    created,
                    baseUrl,
                    User)

            });
        }





        [Authorize(Roles = "CONSUMER,ADMIN")]
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



            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Rating";


            return Ok(new RatingDTOResponse
            {
                Id = updated.Id,

                UserId = updated.UserId,

                MovieId = updated.MovieId,

                Stars = updated.Stars,

                CreatedAt = updated.CreatedAt,


                Links = RatingLinkBuilder.Build(
                    updated,
                    baseUrl,
                    User)

            });
        }





        [Authorize(Roles = "ADMIN")]
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