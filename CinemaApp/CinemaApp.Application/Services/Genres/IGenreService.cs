using CinemaApp.Application.Common.Models;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Services.Genres
{
    public interface IGenreService
    {
        Task<List<Genre>> GetAll();

        Task<PagedResult<Genre>> GetPaged(int page, int pageSize);

        Task<Genre?> GetById(int id);

        Task<Genre?> Create(Genre genre);

        Task<Genre?> Update(int id, Genre genre);

        Task<bool> Delete(int id);
    }
}
