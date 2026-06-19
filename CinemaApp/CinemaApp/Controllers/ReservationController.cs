using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.ReservationsDTO.ReservationDTO;
using CinemaApp.Application.Services.Reservations;
using CinemaApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAll()
        {
            var reservations = await _reservationService.GetAll();

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Reservation";


            return Ok(reservations.Select(r => new ReservationDTOResponse
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
            }));
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



            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/Reservation";


            return Ok(new ReservationDTOResponse
            {
                Id = created.Id,

                UserId = created.UserId,

                GuestEmail = created.GuestEmail,

                MovieScreeningId = created.MovieScreeningId,

                ReservationCode = created.ReservationCode,

                TotalPrice = created.TotalPrice,

                CreatedAt = created.CreatedAt,

                IsCancelled = created.IsCancelled,

                Links = ReservationLinkBuilder.Build(
                    created,
                    baseUrl,
                    User)
            });
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







        [Authorize(Roles = "ADMIN")]
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