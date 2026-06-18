
using CinemaApp.Application.DTO.SeatsDTO;
using CinemaApp.Application.Services.Seats;
using CinemaApp.Domain.Entities;
using CinemaApp.Services.Seats;
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
        public async Task<IActionResult> GetAll()
        {
            var seats = await _service.GetAll();

            return Ok(seats);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var seat = await _service.GetById(id);

            if (seat == null)
                return NotFound();

            return Ok(seat);
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


            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,UpdateSeatDTO dto)
        {
            var seat = new Seat
            {
                Row = dto.Row,
                Number = dto.Number
            };


            var result = await _service.Update(id, seat);


            if (result == null)
                return NotFound();


            return Ok(result);
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
