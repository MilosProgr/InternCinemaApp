using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.DTO.ScreeningSeatDTO
{
    public class ScreeningSeatUpdateDTO
    {
        public bool IsOccupied { get; set; }
        public int? ReservationId { get; set; }
    }
}
