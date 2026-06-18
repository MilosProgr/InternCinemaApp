
using CinemaApp.Application.DTO.SeatsDTO;
using CinemaApp.Application.Services.Seats;
using CinemaApp.Domain.Entities;
using CinemaApp.Services.Seats;
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


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var seats = await _service.GetAll();

            return Ok(seats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var seat = await _service.GetById(id);

            if (seat == null)
                return NotFound();

            return Ok(seat);
        }



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
