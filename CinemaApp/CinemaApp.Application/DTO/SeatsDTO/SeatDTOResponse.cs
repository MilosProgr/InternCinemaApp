using CinemaApp.Application.Common.HATEOAS;

namespace CinemaApp.Application.DTO.SeatDTO
{
    public record SeatDTOResponse
    {
        public int Id { get; set; }


        public string Row { get; set; } = string.Empty;


        public int Number { get; set; }
        public List<Link> Links { get; set; }
    }
}
