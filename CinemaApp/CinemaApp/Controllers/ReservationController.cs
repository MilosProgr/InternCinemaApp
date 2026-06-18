
using CinemaApp.Application.DTO.ReservationsDTO.ReservationDTO;
using CinemaApp.Application.Services.Reservations;
using CinemaApp.Domain.Entities;
using CinemaApp.Services.Reservations;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;


        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reservations = await _reservationService.GetAll();


            return Ok(reservations.Select(r => new ReservationDTOResponse
            {
                Id = r.Id,

                UserId = r.UserId,

                GuestEmail = r.GuestEmail,

                MovieScreeningId = r.MovieScreeningId,

                ReservationCode = r.ReservationCode,

                TotalPrice = r.TotalPrice,

                CreatedAt = r.CreatedAt,

                IsCancelled = r.IsCancelled
            }));
        }





        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _reservationService.GetById(id);


            if (reservation == null)
                return NotFound();



            return Ok(new ReservationDTOResponse
            {
                Id = reservation.Id,

                UserId = reservation.UserId,

                GuestEmail = reservation.GuestEmail,

                MovieScreeningId = reservation.MovieScreeningId,

                ReservationCode = reservation.ReservationCode,

                TotalPrice = reservation.TotalPrice,

                CreatedAt = reservation.CreatedAt,

                IsCancelled = reservation.IsCancelled
            });
        }






        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationDTO dto)
        {
            var reservation = new Reservation
            {
                GuestEmail = dto.GuestEmail,

                MovieScreeningId = dto.MovieScreeningId,

                TotalPrice = 0,

                ReservationCode = Guid.NewGuid().ToString(),

                CreatedAt = DateTime.UtcNow,

                IsCancelled = false
            };


            var created = await _reservationService.Create(reservation);


            if (created == null)
                return BadRequest();



            return Ok(new ReservationDTOResponse
            {
                Id = created.Id,

                GuestEmail = created.GuestEmail,

                MovieScreeningId = created.MovieScreeningId,

                ReservationCode = created.ReservationCode,

                TotalPrice = created.TotalPrice,

                CreatedAt = created.CreatedAt,

                IsCancelled = created.IsCancelled
            });
        }






        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateReservationDTO dto)
        {

            var reservation = new Reservation
            {
                Id = id,


                IsCancelled = dto.IsCancelled
            };


            var result = await _reservationService.Update(id, reservation);



            if (result == null)
                return NotFound();



            return Ok(new ReservationDTOResponse
            {
                Id = result.Id,

                GuestEmail = result.GuestEmail,

                MovieScreeningId = result.MovieScreeningId,

                ReservationCode = result.ReservationCode,

                TotalPrice = result.TotalPrice,

                CreatedAt = result.CreatedAt,

                IsCancelled = result.IsCancelled
            });
        }







        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reservationService.Delete(id);


            if (!result)
                return NotFound();


            return NoContent();
        }
    }
}