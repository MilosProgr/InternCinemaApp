using CinemaApp.Models.DTO.ReservationsDTO;
using CinemaApp.Models.DTO.ReservationsDTO.ReservationSeatDTO;
using CinemaApp.Models.Entities;
using CinemaApp.Services.ReservationSeats;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationSeatController : ControllerBase
    {
        private readonly IReservationSeatService _reservationSeatService;


        public ReservationSeatController(IReservationSeatService reservationSeatService)
        {
            _reservationSeatService = reservationSeatService;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var seats = await _reservationSeatService.GetAll();


            return Ok(seats.Select(s => new ReservationSeatDTOResponse
            {
                Id = s.Id,
                ReservationId = s.ReservationId,
                SeatNumber = s.SeatNumber
            }));
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var seat = await _reservationSeatService.GetById(id);


            if (seat == null)
                return NotFound();


            return Ok(new ReservationSeatDTOResponse
            {
                Id = seat.Id,
                ReservationId = seat.ReservationId,
                SeatNumber = seat.SeatNumber
            });
        }



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


            return Ok(new ReservationSeatDTOResponse
            {
                Id = created.Id,
                ReservationId = created.ReservationId,
                SeatNumber = created.SeatNumber
            });
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateReservationSeatDTO dto)
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


            return Ok(new ReservationSeatDTOResponse
            {
                Id = updated.Id,
                ReservationId = updated.ReservationId,
                SeatNumber = updated.SeatNumber
            });
        }



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