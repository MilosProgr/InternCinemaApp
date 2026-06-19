using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.ReservationsDTO.ReservationSeatDTO;
using CinemaApp.Application.Services.ReservationSeats;
using CinemaApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationSeatController : ControllerBase
    {
        private readonly IReservationSeatService _reservationSeatService;


        public ReservationSeatController(
            IReservationSeatService reservationSeatService)
        {
            _reservationSeatService = reservationSeatService;
        }



        [Authorize(Roles = "ADMIN,CONSUMER")]
        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
        {
            var paged = await _reservationSeatService.GetPaged(page, pageSize);

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/ReservationSeat";

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
                items = paged.Items.Select(s => new ReservationSeatDTOResponse
                {
                    Id = s.Id,
                    ReservationId = s.ReservationId,
                    SeatNumber = s.SeatNumber,

                    Links = ReservationSeatLinkBuilder.Build(
                        s,
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



        [Authorize(Roles = "ADMIN,CONSUMER")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var seat = await _reservationSeatService.GetById(id);

            if (seat == null)
                return NotFound();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/ReservationSeat";


            return Ok(new ReservationSeatDTOResponse
            {
                Id = seat.Id,
                ReservationId = seat.ReservationId,
                SeatNumber = seat.SeatNumber,

                Links = ReservationSeatLinkBuilder.Build(
                    seat,
                    baseUrl,
                    User)
            });
        }



        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationSeatDTO dto)
        {
            var seat = new ReservationSeat
            {
                ReservationId = dto.ReservationId,
                SeatNumber = dto.SeatNumber
            };


            var created = await _reservationSeatService.Create(seat);

            if (created == null)
                return BadRequest();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/ReservationSeat";


            return Ok(new ReservationSeatDTOResponse
            {
                Id = created.Id,
                ReservationId = created.ReservationId,
                SeatNumber = created.SeatNumber,

                Links = ReservationSeatLinkBuilder.Build(
                    created,
                    baseUrl,
                    User)
            });
        }



        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateReservationSeatDTO dto)
        {
            var seat = new ReservationSeat
            {
                Id = id,
                ReservationId = dto.ReservationId,
                SeatNumber = dto.SeatNumber
            };


            var updated = await _reservationSeatService.Update(id, seat);

            if (updated == null)
                return NotFound();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/ReservationSeat";


            return Ok(new ReservationSeatDTOResponse
            {
                Id = updated.Id,
                ReservationId = updated.ReservationId,
                SeatNumber = updated.SeatNumber,

                Links = ReservationSeatLinkBuilder.Build(
                    updated,
                    baseUrl,
                    User)
            });
        }



        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reservationSeatService.Delete(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}