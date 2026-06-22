using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.RatingsDTO;
using CinemaApp.Application.DTO.UsersDTO;
using CinemaApp.Application.Services.Ratings;
using CinemaApp.Domain.Entities;
using CinemaApp.Models.DTO.MoviesDTO.MoviesDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var paged = await _ratingService.GetPaged(page, pageSize);

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Rating";

            var collectionLinks = new List<Link>
            {
                new Link($"{baseUrl}?page={page}&pageSize={pageSize}", "self", "GET")
            };

            if (page > 1)
                collectionLinks.Add(new Link(
                    $"{baseUrl}?page={page - 1}&pageSize={pageSize}", "prev", "GET"));

            if (page < paged.TotalPages)
                collectionLinks.Add(new Link(
                    $"{baseUrl}?page={page + 1}&pageSize={pageSize}", "next", "GET"));

            return Ok(new
            {
                items = paged.Items.Select(r => MapToResponse(r, baseUrl)),
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

            return Ok(MapToResponse(rating, baseUrl));
        }

        [Authorize(Roles = "CONSUMER,ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRatingDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var rating = new Rating
            {
                MovieId = dto.MovieId,
                Stars = dto.Stars,
                UserId = int.Parse(userIdClaim)
            };

            var created = await _ratingService.Create(rating);

            if (created == null)
                return BadRequest(new { message = "Nije moguće oceniti film. Možda već postoji ocena ili nemate prošlu rezervaciju za ovaj film." });

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Rating";

            return Ok(MapToResponse(created, baseUrl));
        }

        [Authorize(Roles = "CONSUMER,ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateRatingDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var requestingUserId = int.Parse(userIdClaim);

            // Prosledi requestingUserId servisu koji proverava vlasništvo
            var isAdmin = User.IsInRole("ADMIN");
            var effectiveUserId = isAdmin ? 0 : requestingUserId; // 0 = preskoči proveru za admina

            var rating = new Rating { Id = id, Stars = dto.Stars };

            var updated = await _ratingService.Update(id, effectiveUserId, rating);

            if (updated == null)
                return NotFound();

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Rating";

            return Ok(MapToResponse(updated, baseUrl));
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

        // ── Privatni helper da se ne ponavlja mapiranje svuda ──────────────
        private RatingDTOResponse MapToResponse(Rating r, string baseUrl)
        {
            return new RatingDTOResponse
            {
                Id = r.Id,
                UserId = r.UserId,
                MovieId = r.MovieId,
                Stars = r.Stars,
                CreatedAt = r.CreatedAt,

                User = r.User == null ? null! : new UserDTOResponse
                {
                    Id = r.User.Id,
                    Username = r.User.Username,
                    FirstName = r.User.FirstName,
                    LastName = r.User.LastName,
                    Email = r.User.Email,
                    Role = r.User.Role
                },

                Movie = r.Movie == null ? null! : new MovieDTOResponse
                {
                    Id = r.Movie.Id,
                    Name = r.Movie.Name,
                    OriginalName = r.Movie.OriginalName,
                    Duration = r.Movie.Duration,
                    PosterUrl = r.Movie.PosterUrl
                },

                Links = RatingLinkBuilder.Build(r, baseUrl, User)
            };
        }
    }
}