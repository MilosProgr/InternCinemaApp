using CinemaApp.Application.Common.HATEOAS;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Application.DTO.GenresDTO
{
    public record GenreDTOResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Link> Links { get; set; } = new();
    }
}


