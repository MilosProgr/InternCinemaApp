using CinemaApp.Application.Common.HATEOAS;
using CinemaApp.Application.DTO.UsersDTO;
using CinemaApp.Application.Services.Users;
using CinemaApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {
            _userService = userService;
        }



        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
        {
            var paged = await _userService.GetPaged(page, pageSize);

            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/User";

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
                items = paged.Items.Select(u => new UserDTOResponse
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role,

                    Links = UserLinkBuilder.Build(
                        u,
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



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);

            if (user == null)
                return NotFound();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/User";


            return Ok(new UserDTOResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,

                Links = UserLinkBuilder.Build(
                    user,
                    baseUrl,
                    User)
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
                PasswordHash = dto.Password
            };


            var created = await _userService.Create(user);


            if (created == null)
                return BadRequest();


            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/User";


            return Ok(new UserDTOResponse
            {
                Id = created.Id,
                FirstName = created.FirstName,
                LastName = created.LastName,
                Username = created.Username,
                Email = created.Email,
                Role = created.Role,

                Links = UserLinkBuilder.Build(
                    created,
                    baseUrl,
                    User)
            });
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateUserDTO dto)
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
            };


            var result = await _userService.Update(id, user);


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