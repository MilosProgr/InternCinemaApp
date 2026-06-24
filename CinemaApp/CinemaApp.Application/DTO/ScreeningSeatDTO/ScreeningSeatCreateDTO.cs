using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.DTO.ScreeningSeatDTO
{
    public class ScreeningSeatCreateDTO
    {
        public string Row { get; set; } = string.Empty;
        public int Number { get; set; }
        public int MovieScreeningId { get; set; }
    }
}
