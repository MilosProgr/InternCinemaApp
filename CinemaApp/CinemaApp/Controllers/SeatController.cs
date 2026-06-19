using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.SeatDTO;
using CinemaApp.Application.DTO.SeatsDTO;
using CinemaApp.Application.Services.Seats;
using CinemaApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _service;


        public SeatController(ISeatService service)
        {
            _service = service;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
        {
            var paged = await _service.GetPaged(page, pageSize);

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Seat";

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
                    new Link(baseUrl, "create", "POST"));
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

            return Ok(new
            {
                items = paged.Items.Select(s => new SeatDTOResponse
                {
                    Id = s.Id,
                    Row = s.Row,
                    Number = s.Number,

                    Links = SeatLinkBuilder.Build(
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


        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var seat = await _service.GetById(id);

            if (seat == null)
                return NotFound();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Seat";


            var result = new SeatDTOResponse
            {
                Id = seat.Id,
                Row = seat.Row,
                Number = seat.Number,

                Links = SeatLinkBuilder.Build(
                    seat,
                    baseUrl,
                    User)
            };


            return Ok(result);
        }



        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateSeatDTO dto)
        {
            var seat = new Seat
            {
                Row = dto.Row,
                Number = dto.Number
            };


            var result = await _service.Create(seat);


            if (result == null)
                return BadRequest("Seat already exists");


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Seat";


            return Ok(new SeatDTOResponse
            {
                Id = result.Id,
                Row = result.Row,
                Number = result.Number,

                Links = SeatLinkBuilder.Build(
                    result,
                    baseUrl,
                    User)
            });
        }



        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateSeatDTO dto)
        {
            var seat = new Seat
            {
                Row = dto.Row,
                Number = dto.Number
            };


            var result = await _service.Update(id, seat);


            if (result == null)
                return NotFound();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Seat";


            return Ok(new SeatDTOResponse
            {
                Id = result.Id,
                Row = result.Row,
                Number = result.Number,

                Links = SeatLinkBuilder.Build(
                    result,
                    baseUrl,
                    User)
            });
        }



        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);


            if (!result)
                return NotFound();


            return NoContent();
        }
    }
}