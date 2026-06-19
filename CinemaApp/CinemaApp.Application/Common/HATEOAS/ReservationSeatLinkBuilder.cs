using CinemaApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.Common.HATEOAS
{
    public static class ReservationSeatLinkBuilder
    {
        public static List<Link> Build(
            ReservationSeat seat,
            string baseUrl,
            ClaimsPrincipal user)
        {
            var links = new List<Link>
            {
                new Link(
                    $"{baseUrl}/{seat.Id}",
                    "self",
                    "GET")
            };


            if (user.IsInRole("ADMIN"))
            {
                links.Add(
                    new Link(
                        $"{baseUrl}/{seat.Id}",
                        "update",
                        "PUT")
                );

                links.Add(
                    new Link(
                        $"{baseUrl}/{seat.Id}",
                        "delete",
                        "DELETE")
                );
            }


            return links;
        }
    }
}
