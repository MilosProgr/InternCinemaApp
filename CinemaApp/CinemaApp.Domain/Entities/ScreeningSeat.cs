using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Entities
{
    public class ScreeningSeat
    {
        public int Id { get; set; }

        public int MovieScreeningId { get; set; }
        public MovieScreening MovieScreening { get; set; } = null!;

        // Red: A, B, C, D, E, F, G
        public string Row { get; set; } = string.Empty;

        // Broj mesta: 1-6
        public int Number { get; set; }

        public bool IsOccupied { get; set; } = false;

        // FK na rezervaciju kad bude zauzeto, null ako slobodno
        public int? ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
    }
}
