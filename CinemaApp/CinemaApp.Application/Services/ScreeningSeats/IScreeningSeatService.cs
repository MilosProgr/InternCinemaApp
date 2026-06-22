using CinemaApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.Services.ScreeningSeats
{
    public interface IScreeningSeatService
    {
        // Sve slobodne i zauzete za prikaz u UI (mreža sedišta)
        Task<IEnumerable<ScreeningSeat>> GetByScreeningIdAsync(int screeningId);

        // Samo slobodna sedišta
        Task<IEnumerable<ScreeningSeat>> GetAvailableByScreeningIdAsync(int screeningId);

        // Interno: generisanje 42 sedišta pri kreiranju projekcije
        Task GenerateSeatsForScreeningAsync(int screeningId);

        // Interno: zauzimanje sedišta pri rezervaciji
        Task OccupySeatAsync(int seatId, int reservationId);

        // Interno: oslobađanje sedišta pri otkazivanju rezervacije
        Task ReleaseSeatAsync(int seatId);
    }
}
