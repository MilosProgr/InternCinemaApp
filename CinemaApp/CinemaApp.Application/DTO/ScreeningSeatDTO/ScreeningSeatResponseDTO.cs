using CinemaApp.Application.Common.HATEOAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.DTO.ScreeningSeatDTO
{
    public class ScreeningSeatResponseDTO
    {
        public int Id { get; set; }
        public string Row { get; set; } = string.Empty;
        public int Number { get; set; }
        public string SeatLabel => $"{Row}{Number}"; // npr. "A3"
        public bool IsOccupied { get; set; }
        public int MovieScreeningId { get; set; }
        public int? ReservationId { get; set; }
        public List<Link> Links { get; set; } = new();
    }
}
