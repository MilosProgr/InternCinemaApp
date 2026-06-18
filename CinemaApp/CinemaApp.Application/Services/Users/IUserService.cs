
using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Services.Users
{
    public interface IUserService
    {
        Task<List<User>> GetAll();

        Task<User?> GetById(int id);

        Task<User?> Create(User user);

        Task<User?> Update(int id,User user);

        Task<bool> Delete(int id);
    }
}
