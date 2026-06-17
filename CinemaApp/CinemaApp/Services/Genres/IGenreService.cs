using CinemaApp.Models.Entities;

namespace CinemaApp.Services.Genres
{
    public interface IGenreService
    {
        Task<List<Genre>> GetAll();

        Task<Genre?> GetById(int id);

        Task<Genre?> Create(Genre genre);

        Task<Genre?> Update(int id, Genre genre);

        Task<bool> Delete(int id);
    }
}
