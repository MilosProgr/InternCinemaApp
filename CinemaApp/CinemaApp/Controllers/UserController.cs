
using CinemaApp.Application.DTO.UsersDTO;
using CinemaApp.Application.Services.Users;
using CinemaApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {
            _userService = userService;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();

            return Ok(users.Select(u => new UserDTOResponse
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role
            }));
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);

            if (user == null)
                return NotFound();


            return Ok(new UserDTOResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            });
        }



        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDTO dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Role = dto.Role,

                // privremeno
                PasswordHash = dto.Password
            };


            var created = await _userService.Create(user);


            if (created == null)
                return BadRequest();


            return Ok(new UserDTOResponse
            {
                Id = created.Id,
                FirstName = created.FirstName,
                LastName = created.LastName,
                Username = created.Username,
                Email = created.Email,
                Role = created.Role
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserDTO dto)
        {
            var user = new User
            {
                Id = id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Role = dto.Role,
                IsVerified = dto.isVerified,
                IsBlocked = dto.IsBlocked
                // privremeno
                //PasswordHash = dto.Password
            };
            var result = await _userService.Update(id,user);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.Delete(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

    }
}