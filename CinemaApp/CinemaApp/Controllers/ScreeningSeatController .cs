using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.ScreeningSeatDTO;
using CinemaApp.Application.Services.ScreeningSeats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;

namespace CinemaApp.Controllers
{
    [ApiController]
    [Route("api/screenings/{screeningId}/seats")]
    public class ScreeningSeatController : ControllerBase
    {
        private readonly IScreeningSeatService _seatService;

        public ScreeningSeatController(IScreeningSeatService seatService)
        {
            _seatService = seatService;
        }

        // GET api/screenings/5/seats
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(int screeningId)
        {
            var seats = await _seatService.GetByScreeningIdAsync(screeningId);
            
            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/screenings/{screeningId}/seats";

            var response = seats.Select(s => new ScreeningSeatResponseDTO
            {
                Id = s.Id,
                Row = s.Row,
                Number = s.Number,
                IsOccupied = s.IsOccupied,
                MovieScreeningId = s.MovieScreeningId,
                ReservationId = s.ReservationId,
                Links = ScreeningSeatLinkBuilder.Build(s, baseUrl, User)
            });

            return Ok(response);
        }

        // ScreeningSeatController.cs
        [HttpPost("generate")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GenerateSeats(int screeningId)
        {
            try
            {
                await _seatService.GenerateSeatsForScreeningAsync(screeningId);
                return Ok(new { message = $"Generisano 42 sedišta za projekciju {screeningId}." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // GET api/screenings/5/seats/available
        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailable(int screeningId)
        {
            var seats = await _seatService.GetAvailableByScreeningIdAsync(screeningId);

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/screenings/{screeningId}/seats";

            var response = seats.Select(s => new ScreeningSeatResponseDTO
            {
                Id = s.Id,
                Row = s.Row,
                Number = s.Number,
                IsOccupied = s.IsOccupied,
                MovieScreeningId = s.MovieScreeningId,
                ReservationId = s.ReservationId,
                Links = ScreeningSeatLinkBuilder.Build(s, baseUrl, User)

            });

            return Ok(response);
        }
    }
}