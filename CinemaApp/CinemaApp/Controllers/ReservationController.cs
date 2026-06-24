using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.MoviesDTO.MovieScreeningsDTO;
using CinemaApp.Application.DTO.ReservationsDTO.ReservationDTO;
using CinemaApp.Application.DTO.ScreeningSeatDTO;
using CinemaApp.Application.Services.Reservations;
using CinemaApp.Domain.Entities;
using CinemaApp.Models.DTO.MoviesDTO.MoviesDTO;
using iText.Svg.Renderers.Path.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CinemaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;


        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var paged = await _reservationService.GetPaged(page, pageSize);

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Reservation";

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
                items = paged.Items.Select(r => new ReservationDTOResponse
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    GuestEmail = r.GuestEmail,
                    MovieScreeningId = r.MovieScreeningId,
                    ReservationCode = r.ReservationCode,
                    TotalPrice = r.TotalPrice,
                    CreatedAt = r.CreatedAt,
                    IsCancelled = r.IsCancelled,

                    Links = ReservationLinkBuilder.Build(
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





        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _reservationService.GetById(id);


            if (reservation == null)
                return NotFound();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Reservation";


            return Ok(new ReservationDTOResponse
            {
                Id = reservation.Id,

                UserId = reservation.UserId,

                GuestEmail = reservation.GuestEmail,

                MovieScreeningId = reservation.MovieScreeningId,

                ReservationCode = reservation.ReservationCode,

                TotalPrice = reservation.TotalPrice,

                CreatedAt = reservation.CreatedAt,

                IsCancelled = reservation.IsCancelled,

                Links = ReservationLinkBuilder.Build(
                    reservation,
                    baseUrl,
                    User)
            });
        }





        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationDTO dto)
        {
            // Uzmi UserId iz tokena ako je ulogovan
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? userId = userIdClaim != null ? int.Parse(userIdClaim) : null;

            // Gost mora da ima email
            if (userId == null && string.IsNullOrEmpty(dto.GuestEmail))
                return BadRequest("Gosti moraju uneti email.");

            var reservation = new Reservation
            {
                UserId = userId,
                GuestEmail = userId == null ? dto.GuestEmail : null,
                MovieScreeningId = dto.MovieScreeningId,
            };

            var created = await _reservationService.Create(reservation, dto.SeatIds);

            if (created == null)
                return BadRequest("Rezervacija nije uspela. Sedišta su možda zauzeta.");

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Reservation";

            return Ok(MapToResponse(created, baseUrl));
        }






        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateReservationDTO dto)
        {
            var reservation = new Reservation
            {
                Id = id,

                IsCancelled = dto.IsCancelled
            };


            var result = await _reservationService.Update(id, reservation);



            if (result == null)
                return NotFound();



            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Reservation";


            return Ok(new ReservationDTOResponse
            {
                Id = result.Id,

                UserId = result.UserId,

                GuestEmail = result.GuestEmail,

                MovieScreeningId = result.MovieScreeningId,

                ReservationCode = result.ReservationCode,

                TotalPrice = result.TotalPrice,

                CreatedAt = result.CreatedAt,

                IsCancelled = result.IsCancelled,

                Links = ReservationLinkBuilder.Build(
                    result,
                    baseUrl,
                    User)
            });
        }

        // Korisnik ili admin može otkazati
        [Authorize]
        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? userId = userIdClaim != null ? int.Parse(userIdClaim) : null;
            bool isAdmin = User.IsInRole("ADMIN");

            var result = await _reservationService.Cancel(id, userId, isAdmin);

            if (result == null)
                return NotFound();

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Reservation";
            return Ok(MapToResponse(result, baseUrl));
        }







        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reservationService.Delete(id);


            if (!result)
                return NotFound();


            return NoContent();
        }

        private ReservationDTOResponse MapToResponse(Reservation r, string baseUrl)
        {
            return new ReservationDTOResponse
            {
                Id = r.Id,
                UserId = r.UserId,
                GuestEmail = r.GuestEmail,
                MovieScreeningId = r.MovieScreeningId,
                ReservationCode = r.ReservationCode,
                TotalPrice = r.TotalPrice,
                CreatedAt = r.CreatedAt,
                IsCancelled = r.IsCancelled,
                Seats = r.Seats.Select(s => new ScreeningSeatResponseDTO
                {
                    Id = s.Id,
                    Row = s.Row,
                    Number = s.Number
                }).ToList(),
                Links = ReservationLinkBuilder.Build(r, baseUrl, User)
            };
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyReservations()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var reservations = await _reservationService.GetMyReservations(userId);

            var result = reservations.Select(r => new ReservationDTOResponse
            {
                Id = r.Id,
                MovieScreeningId = r.MovieScreeningId,
                MovieScreening = new MovieScreeningDTOResponse
                {
                    Id = r.MovieScreening.Id,
                    StartTime = r.MovieScreening.StartTime,
                    TicketPrice = r.MovieScreening.TicketPrice,
                    Movie = new MovieDTOResponse
                    {
                        Id = r.MovieScreening.Movie.Id,
                        Name = r.MovieScreening.Movie.Name,
                        PosterUrl = r.MovieScreening.Movie.PosterUrl
                    }
                },
                ReservationCode = r.ReservationCode,
                TotalPrice = r.TotalPrice,
                CreatedAt = r.CreatedAt,
                IsCancelled = r.IsCancelled,
                Seats = r.Seats.Select(s => new ScreeningSeatResponseDTO
                {
                    Id = s.Id,
                    Row = s.Row,
                    Number = s.Number
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        
    }
}