using CinemaApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.Common.HATEOAS
{
    public static class ReservationLinkBuilder
    {
        public static List<Link> Build(
            Reservation reservation,
            string baseUrl,
            ClaimsPrincipal user)
        {
            var links = new List<Link>
            {
                new Link(
                    $"{baseUrl}/{reservation.Id}",
                    "self",
                    "GET")
            };


            if (user.IsInRole("ADMIN"))
            {
                links.Add(
                    new Link(
                        $"{baseUrl}/{reservation.Id}",
                        "update",
                        "PUT")
                );

                links.Add(
                    new Link(
                        $"{baseUrl}/{reservation.Id}",
                        "delete",
                        "DELETE")
                );
            }


            return links;
        }
    }
}
